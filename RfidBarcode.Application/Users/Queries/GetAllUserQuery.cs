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
    public class GetAllUserQuery : BaseHandler, IRequestHandler<GetAllUserRequest, 
            BaseDataTableResponse<UserVM>>
    {
        public GetAllUserQuery(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<BaseDataTableResponse<UserVM>> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<UserVM>()
            {
                Draw = request.Draw
            };
            var data = new List<UserVM>();
            try
            {
                var list = await _context.Users
                    .Include(x => x.UserRoles)
                        .ThenInclude(x => x.Role).ToListAsync();

                var query = _context.Users
                    .Include(x => x.UserRoles)
                        .ThenInclude(x => x.Role)
                    .Select(x => new UserVM
                    {
                        Id = x.Id,
                        UserName = x.UserName ?? "",
                        Role = x.UserRoles.First() != null ? x.UserRoles.First().Role.Name ?? "" : "",
                    })
                    .AsQueryable();
                var total = query.Count();
                var totalFiltered = total;

                var hasFilter = false;

                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    hasFilter = true;
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => x.UserName.ToLower().Contains(search) ||
                        x.EmployeeNip.ToLower().Contains(search) || 
                        x.EmployeeName.ToLower().Contains(search));
                }

                if (request.Data != null && !string.IsNullOrEmpty(request.Data.UserName))
                {
                    hasFilter = true;
                    var username = request.Data.UserName.ToLower();
                    query = query.Where(x => x.UserName.ToLower().Contains(username));
                }
                if (request.Data != null && !string.IsNullOrEmpty(request.Data.Role))
                {
                    hasFilter = true;
                    var username = request.Data.UserName.ToLower();
                    query = query.Where(x => x.Role.ToLower().Contains(request.Data.Role));
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
