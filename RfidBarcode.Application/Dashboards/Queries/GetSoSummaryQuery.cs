using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Dashboards.Requests;
using RfidBarcode.Application.Dashboards.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Dashboards.Queries
{
    public class GetSoSummaryQuery : IRequestHandler<GetSoSummaryRequest, BaseObjectResponse<SoSummaryVM>>
    {
        private readonly IApplicationDbContext _context;

        public GetSoSummaryQuery(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseObjectResponse<SoSummaryVM>> Handle(GetSoSummaryRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<SoSummaryVM>();

            response.Data = new SoSummaryVM();

            var itemLocation = await _context.StockOpnames
                .Include(x => x.Location)
                .Where(x => x.Location.SkipStockOpname == Location.SKIP_STOCKOPNAME_MODE_NO)
                .Select(x => new
                {
                    LocationId = x.LocationId,
                    LocationName = x.Location != null ? x.Location.Name : "-",
                    CreatedDate = x.CreatedDate
                })
                .GroupBy(x => new
                {
                    x.LocationId,
                    x.LocationName
                })
                .Select(g => new
                {
                    g.Key.LocationId,
                    g.Key.LocationName,
                    LastSoDate = g.Max(x => x.CreatedDate)
                }).OrderBy(x => x.LastSoDate).FirstOrDefaultAsync();
            if (itemLocation != null)
            {
                response.Data.LastCompleteSoDate = itemLocation.LastSoDate != null ? 
                    itemLocation.LastSoDate.Value.ToString("yyyy-MM-dd HH:mm") : "-";
                response.Data.LastCompleteSoLocation = itemLocation.LocationName;
            }

            response.Data.TotalUnknown = await _context.Items
                .Where(x => x.LocationId == null)
                .CountAsync();


            response.Result = BaseResponse.RESULT_OK;

            return response;
        }
    }
}
