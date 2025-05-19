using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Dashboards.Requests;
using RfidBarcode.Application.Settings.Queries;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Crm.Common.ViewModels;
using RfidBarcode.Infrastructure;

namespace RfidBarcode.Crm.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IMqttClientService _mqtt;
        private readonly String _connectionString;

        public IndexModel(IMediator mediator, IMqttClientService mqtt, IConfiguration config)
        {
            _mediator = mediator;
            _mqtt = mqtt;
            _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
        }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostGateStatusAsync()
        {
            var response = new BaseObjectResponse<List<ChartDataVM<int>>>();

            var cmd = new GetAllGateRequest();
            var res = await _mediator.Send(cmd);
            var clientIds = new List<string>();
            if (res.Data != null && res.Data.Count > 0)
            {
                clientIds = [.. res.Data.Select(x => x.ClientId)];
            }

            var countOk = 0;
            var nokGate = "";
            var now = DateTime.Now;
            
            foreach (var clientId in clientIds)
            {
                if (now.AddMinutes(-5) < _mqtt.GetGateLastUpdate(clientId))
                {
                    countOk++;
                }
                else
                {
                    nokGate += "<br/>Gate-" + clientId.Substring(clientId.IndexOf("/") + 1);
                }
            }
            if (nokGate.Length > 0)
            {
                response.Message = "Offline Gate : " + nokGate;
            }
            else
            {
                response.Message = "All Gates are Online";
            }
            response.Result = BaseResponse.RESULT_OK;
            response.Data = new List<ChartDataVM<int>>
            {
                new ChartDataVM<int>() { Name = "Online", Value = countOk },
                new ChartDataVM<int>() { Name = "Offline", Value = (clientIds.Count - countOk) }
            };
            
            return new OkObjectResult(response);
        }


        public async Task<IActionResult> OnPostCheckLocationAsync()
        {
            var cmd = new GetSoSummaryRequest();
            var response = await _mediator.Send(cmd);

            return new OkObjectResult(response);
        }
    }
}
