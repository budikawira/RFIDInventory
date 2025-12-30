using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using Microsoft.EntityFrameworkCore;

namespace RfidBarcode.Application.Users.Handlers
{
    public class DeleteRoleHandler : BaseHandler, IRequestHandler<DeleteRoleRequest,
            BaseResponse>
    {
        public DeleteRoleHandler(IMapper mapper, IApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BaseResponse> Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var entity = await _context.Roles.FindAsync(request.Id);
                if (entity != null)
                {
                    //check if any user use that role 
                    if (await _context.UserRoles.Where(x => x.RoleId == request.Id).AnyAsync())
                    {
                        response.Message = "Role is still assigned to some user.";
                        return response;
                    }

                    _context.Roles.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                    response.Message = "Data removed successfully!";
                    response.Result = BaseResponse.RESULT_OK;
                }
                else
                {
                    response.Message = "Data not found!";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }
            return response;
        }
    }
}
