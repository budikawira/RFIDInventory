using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Crm.Common;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Crm.Pages.SuratJalanP1s
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IMediator mediator, IUserResolverService user) : base(mediator)
        {
            HasAccess = user.HasReadAccess(AccessMenu.SuratJalanOutbond);
        }


        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var request = new GetAllSuratJalanRequest();
            request.InitFromDataTable(Request.Form);
            request.Data = new SuratJalanVM()
            {
                SuratJalanType = SuratJalanType.TYPE_OUTBOND
            };

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
