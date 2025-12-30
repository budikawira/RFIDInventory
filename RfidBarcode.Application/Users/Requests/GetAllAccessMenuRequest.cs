using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Users.ViewModels;

namespace RfidBarcode.Application.Users.Requests
{
    public class GetAllAccessMenuRequest : BaseDataTableRequest<AccessMenuVM>, IRequest<BaseDataTableResponse<AccessMenuVM>>
    {
        public Int32? ExcludedRoleId { get; set; }
        public GetAllAccessMenuRequest()
        {
        }
    }
}
