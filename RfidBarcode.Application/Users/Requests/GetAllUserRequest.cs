using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Users.ViewModels;
using MediatR;

namespace RfidBarcode.Application.Users.Requests
{
    public class GetAllUserRequest : BaseDataTableRequest<UserVM>, IRequest<BaseDataTableResponse<UserVM>>
    {

    }
}
