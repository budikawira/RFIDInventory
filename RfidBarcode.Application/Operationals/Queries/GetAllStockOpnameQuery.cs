using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Operationals.Queries
{
    public class GetAllStockOpnameQuery : BaseHandler, IRequestHandler<GetAllStockOpnameRequest, BaseDataTableResponse<StockOpnameVM>>
    {
        public GetAllStockOpnameQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<StockOpnameVM>> Handle(GetAllStockOpnameRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<StockOpnameVM>()
            {
                Draw = request.Draw
            };

            
            try
            {
                var query = _context.StockOpnames
                    .Include(x => x.StockOpnameDetails)
                    .Include(x => x.Location)
                    .AsNoTracking()
                    .Select(x => new StockOpnameVM
                    {
                        Id = x.Id,
                        CreatedDate = x.CreatedDate ?? DateTime.MinValue,
                        CreatedBy = x.CreatedBy,
                        LastUpdateDate = x.LastUpdateDate ?? DateTime.MinValue,
                        LastUpdateBy = x.LastUpdateBy,
                        LocationId = x.LocationId ?? 0,
                        LocationName = x.FinalLocationName ?? "",
                        Scanned = x.StockOpnameDetails.Where(x => x.Note == "Scanned").Count(),
                        Misplaced = x.StockOpnameDetails.Where(x => x.Note == "Misplaced").Count(),
                        NotScanned = x.StockOpnameDetails.Where(x => x.Note == "Not Scanned").Count(),
                        InvalidTag = x.StockOpnameDetails.Where(x => x.Note == "Invalid Tag").Count(),
                    }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;

                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => x.LocationName.ToLower().Contains(search));
                    totalFiltered = await query.CountAsync();
                }

                if (!string.IsNullOrEmpty(request.SortColumn) && !string.IsNullOrEmpty(request.SortColumn))
                {
                    query = query.OrderBy(request.SortColumn + " " + request.SortColumnDir);
                }
                query = query.Skip(request.Skip);

                if (request.PageSize > 0)
                {
                    query = query.Take(request.PageSize);
                }

                response.Data = await query.ToListAsync();
                response.RecordsTotal = total;
                response.RecordsFiltered = totalFiltered;

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception GetAllStockOpnameQuery: " + ex.Message);
            }

            return response;
        }
    }
}
