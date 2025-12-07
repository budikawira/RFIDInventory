using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Crm.Pages.Inbounds
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
            var request = new GetAllSuratJalanRequest();
            request.InitFromDataTable(Request.Form);
            request.Data = new SuratJalanVM();
            request.Data.SuratJalanType = SuratJalanType.TYPE_INBOUND;

            var response = await _mediator.Send(request);

            return new OkObjectResult(response);
        }
    }
}
