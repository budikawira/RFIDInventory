using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using Serilog;

namespace RfidBarcode.Crm.Settings.Pages.Locations
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;

            Log.Information("User {Username} logged in at {LoginTime}", "JohnDoe", DateTime.UtcNow);
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var response = new BaseDataTableResponse<LocationVM>();
            try
            {
                var request = new GetAllLocationRequest();
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {
                
            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostSaveAsync(LocationVM data)
        {
            var response = new BaseObjectResponse<LocationVM>();
            var command = new CreateLocationRequest(data);

            response = await _mediator.Send(command);


            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var response = new BaseResponse();
            try
            {
                var request = new DeleteLocationRequest(id);
                response = await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return new OkObjectResult(response);
        }
    }
}
