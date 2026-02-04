using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Crm.Common;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Crm.Pages.Inbounds
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IMediator mediator, IUserResolverService user) : base(mediator)
        {
            HasAccess = user.HasReadAccess(AccessMenu.SuratJalanInbound);
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

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var response = new BaseResponse();
            try
            {
                var request = new DeleteSuratJalanRequest(id);
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
