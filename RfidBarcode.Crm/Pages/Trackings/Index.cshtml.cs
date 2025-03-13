using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Crm.Pages.Trackings
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var status = Request.Form["status"].ToString();
            var statusId = int.Parse(status ?? "0");
            var response = new BaseDataTableResponse<TrackingItemVM>();
            try
            {
                var request = new GetAllTrackingItemRequest(statusId);
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }
    }
}
