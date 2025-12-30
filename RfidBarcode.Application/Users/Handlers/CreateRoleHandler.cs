using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Domain.Entities.Identities;

namespace RfidBarcode.Application.Users.Handlers
{
    public class CreateRoleHandler : BaseHandler, IRequestHandler<CreateRoleRequest,
            BaseObjectResponse<RoleVM>>
    {
        private readonly RoleManager<ApplicationRole> _role;
        public CreateRoleHandler(IMapper mapper, IApplicationDbContext context, 
            RoleManager<ApplicationRole> role)
        {
            _context = context;
            _mapper = mapper;
            _role = role;
        }

        public async Task<BaseObjectResponse<RoleVM>> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<RoleVM>();

            if (request.Data.Id == 0)
            {
                var entity = new ApplicationRole()
                {
                    Name = request.Data.Name,
                    NormalizedName = request.Data.Name.ToUpper()
                };
                _role.CreateAsync(entity).Wait();
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Create role successful!";
                request.Data.Id = entity.Id;
                response.Data = request.Data;
            }
            else
            {
                //update
                var entity = await _context.Roles.FindAsync(request.Data.Id);
                if (entity != null)
                {
                    entity.Name = request.Data.Name;
                    entity.NormalizedName = request.Data.Name.ToUpper();
                    _role.UpdateAsync(entity).Wait();
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Update role successful!";
                    response.Data = request.Data;
                }
                else
                {
                    response.Message = "Role not found!";
                }
            }
            return response;
        }
    }
}
