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
    public class GetAllStockOpnameDetailQuery : BaseHandler, IRequestHandler<GetAllStockOpnameDetailRequest, 
        BaseDataTableResponse<StockOpnameDetailVM>>
    {
        public GetAllStockOpnameDetailQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<StockOpnameDetailVM>> Handle(GetAllStockOpnameDetailRequest request, 
            CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<StockOpnameDetailVM>()
            {
                Draw = request.Draw
            };

            
            try
            {
                var query = _context.StockOpnameDetails
                    .Include(x => x.Item)
                    .AsNoTracking()
                    .Select(x => new StockOpnameDetailVM
                    {
                        Id = x.Id,
                        StockOpnameId = x.StockOpnameId,
                        CreatedDate = x.CreatedDate ?? DateTime.MinValue,
                        CreatedBy = x.CreatedBy,
                        LastUpdateDate = x.LastUpdateDate ?? DateTime.MinValue,
                        LastUpdateBy = x.LastUpdateBy,
                        Note = x.Note,
                        TagId = x.TagId,
                        ItemId = x.ItemId,
                        Merk = x.Item != null ? x.Item.Merk : "",
                        Kp = x.Item != null ? x.Item.Kp : "",
                        Kode = x.Item != null ? x.Item.Kode : "",
                        SerialNumber = x.Item != null ? x.Item.SerialNumber : ""
                    }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;
                if (request.StockOpnameId != null)
                {
                    query = query.Where(x => x.StockOpnameId == request.StockOpnameId);
                }
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => x.TagId.ToLower().Contains(search) || 
                        (x.Merk != null && x.Merk.ToLower().Contains(search)) ||
                        (x.Kp != null && x.Kp.ToLower().Contains(search)) ||
                        (x.Kode != null && x.Kode.ToLower().Contains(search)) ||
                        (x.SerialNumber != null && x.SerialNumber.ToLower().Contains(search)));
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
                Console.Error.WriteLine("Exception GetAllStockOpnameDetailQuery: " + ex.Message);
            }

            return response;
        }
    }
}
