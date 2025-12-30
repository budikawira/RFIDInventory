using MediatR;
using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Users.Requests
{
    public class DeleteAccessMenuRoleRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }
        public DeleteAccessMenuRoleRequest(long id)
        {
            Id = id;
        }
    }
}
