using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Users.Queries
{
    public class GetAllAccessMenuQuery : BaseHandler, IRequestHandler<GetAllAccessMenuRequest,
            BaseDataTableResponse<AccessMenuVM>>
    {
        public GetAllAccessMenuQuery(IMapper mapper, IApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BaseDataTableResponse<AccessMenuVM>> Handle(GetAllAccessMenuRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<AccessMenuVM>()
            {
                Draw = request.Draw
            };
            var data = new List<AccessMenuVM>();
            try
            {
                var query = _context.AccessMenus
                    .Include(x => x.AccessMenuRoles)
                    .AsQueryable();
                var total = query.Count();
                var totalFiltered = total;

                if (request.ExcludedRoleId != null)
                {

                    query = query.Where(x => !x.AccessMenuRoles.Where(x => x.RoleId == request.ExcludedRoleId).Any());
                }

                var qry1 = query.Select(x => new AccessMenuVM
                {
                    Id = x.Id,
                    Description = x.Description,
                });
                var hasFilter = false;
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    hasFilter = true;
                    var search = request.SearchValue.ToLower();
                    qry1 = qry1.Where(x => x.Description.ToLower().Contains(search));
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

                data = await qry1.ToListAsync();
                response.RecordsTotal = total;
                response.RecordsFiltered = totalFiltered;
                response.Data = data;
            }
            catch (Exception) { }

            return response;
        }
    }
}
