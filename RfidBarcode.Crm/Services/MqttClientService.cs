using DocumentFormat.OpenXml.Vml.Office;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Newtonsoft.Json;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Services;
using RfidBarcode.Infrastructure;
using Serilog;
using System.Diagnostics;
using System.Text;

namespace RfidBarcode.Crm.Services
{
    public class MqttClientService : IMqttClientService
    {
        private ManagedMqttClientOptions _mqttOptions;
        private IManagedMqttClient _mqttClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private ICollection<Gate> gates;
        //mapping of topic + "/" + antena to LocationId
        private Dictionary<string, LocationVM> mapLocations = new Dictionary<string, LocationVM>();

        private List<MqttTopicFilter> topics;
        private List<TagScannedLog> tagScannedLogs = new List<TagScannedLog>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private Dictionary<String, long> GateStatusUpdate = new Dictionary<String, long>();

        public MqttClientService(IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            Random rnd = new Random();
            _mqttClient = new MqttFactory().CreateManagedMqttClient();
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                        .WithClientId(config["Mqtt:ClientId"] + rnd.Next(0, 100))
                                        .WithTcpServer(config["Mqtt:Broker"], int.Parse(config["Mqtt:Port"]!))
                                        .WithCredentials(config["Mqtt:Username"], config["Mqtt:Password"]);
            _mqttOptions = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                                    .WithClientOptions(builder.Build())
                                    .Build();
            gates = new List<Gate>();
            topics = new List<MqttTopicFilter>();

            _mqttClient.ConnectedAsync += mqttClient_ConnectedAsync;
            _mqttClient.DisconnectedAsync += mqttClient_DisconnectedAsync;
            _mqttClient.ConnectingFailedAsync += mqttClient_ConnectingFailedAsync;
            _mqttClient.ApplicationMessageReceivedAsync += mqttClient_ApplicationMessageReceivedAsync;

            //LatestGateControllerSubmitTimes = new Dictionary<string, DateTime>();
        }


        public async Task UpdateGates()
        {
            //LatestGateSubmitTimes.Clear();
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    gates = await context.Gates.Include(x => x.GateMaps)
                        .ThenInclude(x => x.NextLocation)
                        .ToListAsync();
                    mapLocations.Clear();
                    foreach (var gate in gates)
                    {
                        foreach (var gateMap in gate.GateMaps)
                        {
                            mapLocations.Add(gate.ClientId + "/" + gateMap.Antenna, 
                                new LocationVM() { Id = gateMap.NextLocationId, Name = gateMap.NextLocation.Name ?? "" });
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Exception : " + e.Message);
                }
            }
        }

        public Task EnqueueAsync(ManagedMqttApplicationMessage msg)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetGateLastUpdate(string clientId)
        {
            var unixTimeMillis = GateStatusUpdate.GetValueOrDefault(clientId);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMillis);
            DateTime dateTime = dateTimeOffset.LocalDateTime; // or .UtcDateTime if you prefer UTC
            return dateTime;
        }

        public async Task mqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            //susbcribe
            topics = new List<MqttTopicFilter>();
            GateStatusUpdate.Clear();
            foreach (var gate in gates)
            {
                var topicFilter = new MqttTopicFilterBuilder()
                        .WithTopic(gate.ClientId)
                        .WithExactlyOnceQoS() // <-- This sets the QoS of the subscription
                        .Build();
                topics.Add(topicFilter);
                GateStatusUpdate.Add(gate.ClientId, 0);
            }

            await _mqttClient.SubscribeAsync(topics);
        }


        public bool IsConnected()
        {
            return _mqttClient.IsConnected;
        }

        public Task mqttClient_ConnectingFailedAsync(ConnectingFailedEventArgs arg)
        {
            Log.Information("ConnectingFailed: " + arg.Exception.Message);
            Debug.WriteLine("ConnectingFailed: " + arg.Exception.Message);
            return Task.CompletedTask;
        }

        public Task mqttClient_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await UpdateGates();
                await _mqttClient.StartAsync(_mqttOptions);
            }
            catch (Exception e)
            {
                //Log.Information("StartAsync : Connect Exception : " + e.Message);
                Console.WriteLine("Failed connection : " + e.Message);
            }
            finally
            {
                //Log.CloseAndFlush();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _mqttClient.StopAsync(true);
        }

        public async Task Subscribe(string topic)
        {
            await _mqttClient.SubscribeAsync(topic);
        }

        public async Task Unsubscribe(string topic)
        {
            //GateStatusUpdate.Remove(topic);
            await _mqttClient.UnsubscribeAsync(topic);
        }

        private async Task ProcessGateData(string topic, GateData gateData)
        {
            var updateLogs = new List<TagScannedLog>();
            if (await _semaphore.WaitAsync(TimeSpan.FromMinutes(1)))
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                        foreach (var tagSummary in gateData.Data)
                        {
                            LocationVM? location = null;
                            var prevAntenna = "";
                            foreach (var data in tagSummary.Data)
                            {
                                if (prevAntenna != data.Ant)
                                {
                                    if (mapLocations.TryGetValue(topic + "/" + data.Ant, out location))
                                    {
                                        prevAntenna = data.Ant;
                                    }
                                    else
                                    {
                                        Log.Information("LOCATION NOT FOUND! SKIP LOG");
                                        Debug.WriteLine("LOCATION NOT FOUND! SKIP LOG");
                                        continue;
                                    }
                                }

                                var log = tagScannedLogs.Where(x => x.Epc == tagSummary.Epc).FirstOrDefault();

                                //existing tag already detected
                                if (log != null && location != null)
                                {
                                    if (log.LocationId == location.Id)
                                    {
                                        //still in the same location
                                        if (data.Time - log.LastScanned < 5000)
                                        {
                                            //tag is still in location
                                            log.LastScanned = data.Time;
                                            if (log.Id == 0)
                                            {
                                                if (log.LastScanned - log.Start > 1000)
                                                {
                                                    //detected for more than 1 second
                                                    log.End = log.LastScanned;
                                                    log = await UpdateTagLocationLog(mediator, context, log);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //tag already gone, record in database and renew the data
                                            log.End = log.LastScanned;
                                            await UpdateTagLocationLog(mediator, context, log);
                                            log = new TagScannedLog();
                                            log.Start = data.Time;
                                            log.LastScanned = data.Time;
                                        }
                                    }
                                    else
                                    {
                                        //tag already change location
                                        //how to ensure that it is not a false location detection ?
                                        //if it is not detected in previous location for more than 1 second
                                        //then assume that it is already a correct data
                                        if (data.Time - log.End > 1000 || log.Id == 0)
                                        {
                                            log.End = log.LastScanned;
                                            await UpdateTagLocationLog(mediator, context, log);
                                            log = new TagScannedLog();
                                            log.LocationId = location.Id;
                                            log.LocationName = location.Name;
                                            log.Start = data.Time;
                                            log.End = 0;
                                            log.LastScanned = data.Time;
                                            log.Id = 0;
                                        }

                                    }
                                }
                                else
                                {
                                    var newLog = new TagScannedLog()
                                    {
                                        Epc = tagSummary.Epc,
                                        Start = data.Time,
                                        End = null,
                                        LastScanned = data.Time,
                                        Id = 0,
                                        LocationId = location != null ? location.Id : 0,
                                        LocationName = location != null ? location.Name : ""
                                    };
                                    tagScannedLogs.Add(newLog);
                                }
                            }
                        }

                        var removedLogs = tagScannedLogs.Where(x => x.LastScanned < (gateData.Time - 5000)).ToList();
                        Log.Information("Removed Logs : " + removedLogs.Count + ", Offset : " + (gateData.Time - 5000).ToString());
                        Debug.WriteLine("Removed Logs : " + removedLogs.Count + ", Offset : " + (gateData.Time - 5000).ToString());
                        foreach (var log in removedLogs)
                        {
                            log.End = log.LastScanned;
                            await UpdateTagLocationLog(mediator, context, log);
                        }
                        tagScannedLogs.RemoveAll(x => x.LastScanned < (gateData.Time - 5000));
                    }

                    
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception : " + ex.Message);
                }
                finally
                {
                    _semaphore.Release();
                }
                
            }
            else
            {
                Debug.WriteLine("Timeout: Could not acquire semaphore");
            }
        }

        private async Task<TagScannedLog> UpdateTagLocationLog(IMediator mediator, IApplicationDbContext context, TagScannedLog log)
        {
            try
            {
                if (log.Id >= 0)
                {
                    if (log.ItemId == 0)
                    {

                        var cmdItem = new GetItemRequest(new ItemVM() { Epc = log.Epc });
                        var resItem = await mediator.Send(cmdItem);
                        if (resItem.Result == BaseResponse.RESULT_OK && resItem.Data != null && resItem.Data.SuratJalanP1Id == null)
                        {
                            log.ItemId = resItem.Data.Id;

                            //update item location
                            var prevLocationId = resItem.Data.LocationId;
                            var newLocationId = log.LocationId;
                            
                            if (prevLocationId != newLocationId)
                            {
                                var itemMovement = new ItemMovement()
                                {
                                    ItemId = log.ItemId,
                                    PrevLocationId = prevLocationId,
                                    LocationId = newLocationId,
                                    PrevLocationName = resItem.Data.LocationName,
                                    LocationName = log.LocationName,
                                    Source = ItemMovement.SOURCE_GATE,
                                    TagLocationId = log.Id
                                };
                                await context.ItemMovements.AddAsync(itemMovement);
                            }

                            //update the item location
                            resItem.Data.LocationId = log.LocationId;
                            if (resItem.Data.PrintCount == 0)
                            {
                                var list = new List<long>();
                                list.Add(resItem.Data.Id);
                                var cmd3 = new CreateItemPrintLogsRequest(list);
                                var res3 = await mediator.Send(cmd3);
                            }
                            var cmd2 = new CreateItemRequest(resItem.Data);
                            var res2 = await mediator.Send(cmd2);

                        }
                        else
                        {
                            log.Id = -1;
                            return log;
                        }
                    }

                    var vm = new TagLocationVM()
                    {
                        Id = log.Id,
                        Epc = log.Epc,
                        StartScanned = DateTimeOffset.FromUnixTimeMilliseconds(log.Start).UtcDateTime,
                        EndScanned = log.End != null ? DateTimeOffset.FromUnixTimeMilliseconds((long)log.End!).UtcDateTime : null,
                        LastScanned = DateTimeOffset.FromUnixTimeMilliseconds(log.LastScanned).UtcDateTime,
                        ItemId = log.ItemId,
                        LocationId = log.LocationId
                    };
                    
                    var cmd = new CreateTagLocationRequest(vm);
                    var res = await mediator.Send(cmd);
                    if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
                    {
                        log.Id = res.Data.Id;
                    }
                    else
                    {
                        log.Id = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Id = -1; //error, skip processing this data
                Debug.WriteLine("Exception!! " + ex.Message);
                Log.Information("Exception!! " + ex.Message);

            }
            return log;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task mqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (e.ApplicationMessage.Topic.StartsWith("g/"))
            {
                if (e != null && e.ApplicationMessage != null)
                {
                    var content = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                    try
                    {
                        var gateData = JsonConvert.DeserializeObject<GateData>(content);
                        if (gateData != null)
                        {
                            await ProcessGateData(e.ApplicationMessage.Topic, gateData);

                            if (GateStatusUpdate.ContainsKey(e.ApplicationMessage.Topic))
                            {
                                var prevValue = GateStatusUpdate[e.ApplicationMessage.Topic];
                                if (prevValue < gateData.Time)
                                {
                                    GateStatusUpdate[e.ApplicationMessage.Topic] = gateData.Time;
                                }
                            }
                        }
                        //if (gateLog != null)
                        {
                            //await StoreGateLogAsync(gateLog);
                            //await ProcessGateLogAsync(gateLog.Topic);
                        }
                        //Debug.WriteLine(e.ApplicationMessage.Topic + " : " + content);
                        Debug.WriteLine("tagScannedLog : " + JsonConvert.SerializeObject(tagScannedLogs));
                        Log.Information("tagScannedLog : " + JsonConvert.SerializeObject(tagScannedLogs));

                    }
                    catch (Exception) { }
                }
            }
            else if (e.ApplicationMessage.Topic == "sc")
            {
                var content = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                try
                {
                    //var result = JsonConvert.DeserializeObject<GateStatus>(content);
                    //if (result != null && GateStatusUpdate.ContainsKey(result.ClientId))
                    //{
                    //    GateStatusUpdate[result.ClientId] = result.LastPackageReceived;
                    //}
                }
                catch (Exception) { }
            }
        }
    }
}
