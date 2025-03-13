using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Users.ViewModels;
using MediatR;

namespace RfidBarcode.Application.Users.Requests
{
    public class CreateUserRequest : IRequest<BaseObjectResponse<UserVM>>
    {
        public UserVM Data { get; set; }

        public CreateUserRequest(UserVM data)
        {
            Data = data;
        }
    }
}
