using AutoMapper;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Users.Queries
{
    public class GetAllRoleQuery : BaseHandler, IRequestHandler<GetAllRoleRequest, 
            BaseDataTableResponse<RoleVM>>
    {
        public GetAllRoleQuery(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<BaseDataTableResponse<RoleVM>> Handle(GetAllRoleRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<RoleVM>()
            {
                Draw = request.Draw
            };
            var data = new List<RoleVM>();
            try
            {
                var query = _context.Roles
                    .Select(x => new RoleVM
                    {
                        Id = x.Id,
                        Name = x.Name ?? ""
                    })
                    .AsQueryable();
                var total = query.Count();
                var totalFiltered = total;

                var hasFilter = false;
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    hasFilter = true;
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(search));
                }


                if (hasFilter)
                {
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
                
                data = await query.ToListAsync();
                response.RecordsTotal = total;
                response.RecordsFiltered = totalFiltered;
                response.Data = data;
            }
            catch (Exception) { }

            return response;
        }
    }
}
