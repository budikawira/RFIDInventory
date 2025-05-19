using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.Responses;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Crm.Common.ViewModels.Api.Requests;
using System.Globalization;

namespace RfidBarcode.Crm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        public async Task<BaseObjectResponse<ItemVM>> OnPostAsync(ItemRequest request)
        {
            var cmd = new GetItemRequest(new ItemVM() { Id = request.Id });
            var res = await _mediator.Send(cmd);

            return res;
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [Route("SuratJalanP1")]
        public async Task<CreateMultipleSuratJalanP1Response> OnPostSuratJalanP1Async(CreateMultipleSuratJalanP1Request request)
        {
            var res = await _mediator.Send(request);

            return res;
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [Route("FindTable")]
        public async Task<BaseDataTableResponse<ItemVM>> OnPostFindTableAsync(GetAllItemRequest request)
        {
            request.SortColumn = "TanggalBuatBarcode";
            request.SortColumnDir = "desc";
            request.PageSize = 1000;
            var res = await _mediator.Send(request);
         
            return res;
        }

    }
}
