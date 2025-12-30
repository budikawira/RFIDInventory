using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;

namespace RfidBarcode.Application.Users.Handlers
{
    public class DeleteAccessMenuRoleHandler : BaseHandler, IRequestHandler<DeleteAccessMenuRoleRequest,
            BaseResponse>
    {
        public DeleteAccessMenuRoleHandler(IMapper mapper, IApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteAccessMenuRoleRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var entity = await _context.AccessMenuRoles.FindAsync(request.Id);
                if (entity != null)
                {
                    _context.AccessMenuRoles.Remove(entity);
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
