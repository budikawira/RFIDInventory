using MediatR;
using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Users.Requests
{
    public class CreateAccessMenuRoleRequest : IRequest<BaseResponse>
    {
        public long RoleId { get; set; }
        public List<string> AccessMenuIds { get; set; }

        public CreateAccessMenuRoleRequest(long roleId, List<string> accessMenuIds)
        {
            RoleId = roleId;
            AccessMenuIds = accessMenuIds;
        }
    }
}
