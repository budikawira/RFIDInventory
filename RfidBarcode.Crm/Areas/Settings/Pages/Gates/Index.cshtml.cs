using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Crm.Areas.Settings.Pages.Gates
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var response = new BaseDataTableResponse<GateVM>();
            try
            {
                var request = new GetAllGateRequest();
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var response = new BaseResponse();
            var command = new DeleteGateRequest(id);

            response = await _mediator.Send(command);


            return new OkObjectResult(response);
        }
    }
}
