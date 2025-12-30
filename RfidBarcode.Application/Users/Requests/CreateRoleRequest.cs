using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Users.ViewModels;

namespace RfidBarcode.Application.Users.Requests
{
    public class CreateRoleRequest : IRequest<BaseObjectResponse<RoleVM>>
    {
        public RoleVM Data { get; set; }

        public CreateRoleRequest(RoleVM data)
        {
            Data = data;
        }
    }
}
