using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using Serilog;

namespace RfidBarcode.Crm.Areas.Settings.Pages.SuratJalans
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
            var response = new BaseDataTableResponse<SuratJalanTypeVM>();
            try
            {
                var request = new GetAllSuratJalanTypeRequest();
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }


        public async Task<IActionResult> OnPostSaveAsync(SuratJalanTypeVM data)
        {
            var response = new BaseObjectResponse<SuratJalanTypeVM>();
            var command = new CreateSuratJalanTypeRequest(data);

            response = await _mediator.Send(command);


            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var response = new BaseResponse();
            try
            {
                var request = new DeleteSuratJalanTypeRequest(id);
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
