using AutoMapper;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Domain.Entities.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Users.Queries
{
    public class CreateUserHandler : BaseHandler, IRequestHandler<CreateUserRequest, 
            BaseObjectResponse<UserVM>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public CreateUserHandler(IMapper mapper, IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }
        public async Task<BaseObjectResponse<UserVM>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<UserVM>();

            if (request.Data.Id == 0)
            {
                //create
                var hasher = new PasswordHasher<IdentityUser>();
                var user = new ApplicationUser
                {
                    UserName = request.Data.UserName,
                    NormalizedUserName = request.Data.UserName.ToUpper(),
                    PasswordHash = hasher.HashPassword(null!, request.Data.Password ?? ""),
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await _context.Users.AddAsync(user);

                var role = await _context.Roles.Where(x => x.Name == request.Data.Role).FirstOrDefaultAsync();
                if (role != null)
                {
                    var userRole = new ApplicationUserRole
                    {
                        Role = role,
                        User = user
                    };
                    await _context.UserRoles.AddAsync(userRole);
                }

                await _context.SaveChangesAsync(cancellationToken);
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Create user successful!";
                request.Data.Id = user.Id;
                response.Data = request.Data;
            }
            else
            {
                //update
                var user = await _context.Users.FindAsync(request.Data.Id);
                if (user != null)
                {
                    var isInRole = await _userManager.IsInRoleAsync(user, request.Data.Role);
                    if (!isInRole)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, roles);
                        await _userManager.AddToRoleAsync(user, request.Data.Role);
                    }
                    user.UserName = request.Data.UserName;
                    if (!string.IsNullOrEmpty(request.Data.Password))
                    {
                        var hasher = new PasswordHasher<IdentityUser>();
                        user.PasswordHash = hasher.HashPassword(null!, request.Data.Password ?? "");
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Update user successful!";
                    response.Data = request.Data;
                }
                else
                {
                    response.Message = "User not found!";
                }
            }
            return response;
        }
    }
}
