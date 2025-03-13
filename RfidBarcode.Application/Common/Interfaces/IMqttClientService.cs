using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace RfidBarcode.Application.Common.Interfaces
{
    public interface IMqttClientService : IHostedService
    {
        public bool IsConnected();

        public Task Subscribe(string topic);
        public Task Unsubscribe(string topic);

        public Task EnqueueAsync(ManagedMqttApplicationMessage msg);

        public Task mqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg);
        public Task mqttClient_DisconnectedAsync(MqttClientDisconnectedEventArgs arg);
        public Task mqttClient_ConnectingFailedAsync(ConnectingFailedEventArgs arg);
        public DateTime? GetGateLastUpdate(string clientId);
    }
}
