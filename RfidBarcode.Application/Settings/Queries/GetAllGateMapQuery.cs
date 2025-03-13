using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Settings.Queries
{
    public class GetAllGateMapQuery : BaseHandler, IRequestHandler<GetAllGateMapRequest, BaseDataTableResponse<GateMapVM>>
    {
        public GetAllGateMapQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<GateMapVM>> Handle(GetAllGateMapRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<GateMapVM>()
            {
                Draw = request.Draw
            };


            try
            {
                var query = _context.GateMaps
                    .Include(x => x.Gate)
                    .Include(x => x.NextLocation)
                    .Include(x => x.PrevLocation)
                    .AsNoTracking()
                    .Select(a => new GateMapVM
                    {
                        Id = a.Id,
                        Antenna = a.Antenna,
                        NextLocationId = a.NextLocationId,
                        PrevLocationId = a.PrevLocationId,
                        NextLocationName = a.NextLocation != null ? a.NextLocation.Name : "",
                        PrevLocationName = a.PrevLocation != null ? a.PrevLocation.Name : ""
                    }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;

                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => (x.Antenna != null && x.Antenna.ToLower().Contains(search)) || x.PrevLocationName.ToLower().Contains(search) || 
                        x.NextLocationName.ToLower().Contains(search));
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
                Console.Error.WriteLine("Exception GetAllGateMapQuery: " + ex.Message);
            }
            return response;
        }
    }
}
