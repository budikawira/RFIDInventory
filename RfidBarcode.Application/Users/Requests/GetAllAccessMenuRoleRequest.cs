using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;

namespace RfidBarcode.Application.Users.Requests
{
    public class GetAllAccessMenuRoleRequest : BaseDataTableRequest<AccessMenuVM>, 
        IRequest<BaseDataTableResponse<AccessMenuRoleVM>>
    {
        public Int32 RoleId { get; set; }
        public GetAllAccessMenuRoleRequest(int roleId)
        {
            RoleId = roleId;
        }
    }
}
