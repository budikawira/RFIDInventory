using MediatR;
using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Users.Requests
{
    public class DeleteRoleRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }
        public DeleteRoleRequest(long id)
        {
            Id = id;
        }
    }
}
