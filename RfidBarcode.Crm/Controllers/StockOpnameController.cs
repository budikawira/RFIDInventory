using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.Responses;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Crm.Common.ViewModels.Api.Requests;
using RfidBarcode.Crm.Common.ViewModels.Api.Responses;
using RfidBarcode.Infrastructure;
using System.Globalization;

namespace RfidBarcode.Crm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockOpnameController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserResolverService _user;
        public StockOpnameController(IMediator mediator, IUserResolverService user)
        {
            _mediator = mediator;
            _user = user;
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [Route("ItemByLocation")]
        public async Task<ItemTable> OnPostItemByLocationAsync(ItemByLocationRequest request)
        {
            var res = new ItemTable();
            var cmd = new GetAllItemRequest()
            {
                LocationId = request.LocationId
            };

            var response = await _mediator.Send(cmd);

            res.RecordsTotal = response.Data.Count;
            res.Data = response.Data;

            res.Result = ItemTable.RESULT_SUCCESS;
            res.Message = "";

            return res;
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [Route("ItemUpload")]
        public async Task<StockOpnameUploadResponse> OnPostItemUploadAsync(StockOpnameUploadRequest request)
        {
            request.UserId = _user.GetUserId();
            var res = await _mediator.Send(request);

            return res;
        }
    }
}
