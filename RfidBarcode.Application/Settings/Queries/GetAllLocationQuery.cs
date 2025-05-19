using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Settings.Queries
{
    public class GetAllLocationQuery : BaseHandler, IRequestHandler<GetAllLocationRequest, BaseDataTableResponse<LocationVM>>
    {
        public GetAllLocationQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<LocationVM>> Handle(GetAllLocationRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<LocationVM>()
            {
                Draw = request.Draw
            };


            try
            {
                var query = _context.Locations
                    .AsNoTracking()
                    .Select(a => new LocationVM
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description,
                        Type = a.Type,
                        SkipStockOpname = a.SkipStockOpname
                    }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;

                if (request.IsForStockOpname)
                {
                    query = query.Where(x => x.SkipStockOpname == Location.SKIP_STOCKOPNAME_MODE_NO);
                }
                if (request.IsForSummaryK3)
                {
                    query = query.Where(x => x.Type == Location.TYPE_END_LOCATION);
                }
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(search) || 
                        (x.Description != null && x.Description.ToLower().Contains(search)));
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
                Console.Error.WriteLine("Exception GetAllItemQuery: " + ex.Message);
            }
            return response;
        }
    }
}
