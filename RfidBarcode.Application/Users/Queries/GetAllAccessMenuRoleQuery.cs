using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Users.Queries
{
    public class GetAllAccessMenuRoleQuery : BaseHandler, IRequestHandler<GetAllAccessMenuRoleRequest,
            BaseDataTableResponse<AccessMenuRoleVM>>
    {
        public GetAllAccessMenuRoleQuery(IMapper mapper, IApplicationDbContext context) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<AccessMenuRoleVM>> Handle(GetAllAccessMenuRoleRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<AccessMenuRoleVM>()
            {
                Draw = request.Draw
            };
            var data = new List<AccessMenuRoleVM>();
            try
            {
                var query = _context.AccessMenuRoles
                    .Include(x => x.AccessMenu)
                    .Where(x => x.RoleId == request.RoleId)
                    .Select(x => new AccessMenuRoleVM()
                    {
                        Id = x.Id,
                        RoleId = x.RoleId,
                        AccessMenuId = x.AccessMenuId,
                        Description = x.AccessMenu.Description
                    })
                    .AsQueryable();
                var total = query.Count();
                var totalFiltered = total;

                var hasFilter = false;
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    hasFilter = true;
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => x.Description.ToLower().Contains(search));
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
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
    }
}
