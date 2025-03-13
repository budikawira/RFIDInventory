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
    public class GetAllGateQuery : BaseHandler, IRequestHandler<GetAllGateRequest, BaseDataTableResponse<GateVM>>
    {
        public GetAllGateQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<GateVM>> Handle(GetAllGateRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<GateVM>()
            {
                Draw = request.Draw
            };


            try
            {
                var query = _context.Gates
                    .AsNoTracking()
                    .Select(a => new GateVM
                    {
                        Id = a.Id,
                        Name = a.Name,
                        ClientId = a.ClientId,
                    }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;

                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(search) || x.ClientId.ToLower().Contains(search));
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
                Console.Error.WriteLine("Exception GetAllGateQuery: " + ex.Message);
            }
            return response;
        }
    }
}
