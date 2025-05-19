using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Dashboards.Requests;
using RfidBarcode.Application.Dashboards.ViewModels;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Crm.Common.ViewModels.Api;
using RfidBarcode.Crm.Common.ViewModels.Api.Responses;

namespace RfidBarcode.Crm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = "Jwt")]
        [Route("Table")]
        public async Task<LocationTable> OnPostTableAsync(GetAllLocationRequest request)
        {
            var res = new LocationTable();
            request.IsForStockOpname = true;
            var result = await _mediator.Send(request);

            if (result.Data != null)
            {
                foreach (var dt in result.Data)
                {
                    res.Data.Add(new DataLocationTable() { Id = dt.Id, Name = dt.Name });
                }
                res.RecordsTotal = res.Data.Count;
                res.Result = LocationTable.RESULT_SUCCESS;
            }
            return res;
        }

    }
}
