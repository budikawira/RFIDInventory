using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Domain.Entities.Identities;

namespace RfidBarcode.Application.Users.Handlers
{
    public class CreateAccessMenuRoleHandler : BaseHandler, IRequestHandler<CreateAccessMenuRoleRequest,
            BaseResponse>
    {

        public CreateAccessMenuRoleHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BaseResponse> Handle(CreateAccessMenuRoleRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                foreach (var accessMenuId in request.AccessMenuIds)
                {
                    var accessMenu = new AccessMenuRole()
                    {
                        AccessMenuId = accessMenuId,
                        RoleId = request.RoleId,
                    };
                    _context.AccessMenuRoles.Add(accessMenu);
                }
                await _context.SaveChangesAsync(cancellationToken);
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Berhasil menambahkan access role!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
