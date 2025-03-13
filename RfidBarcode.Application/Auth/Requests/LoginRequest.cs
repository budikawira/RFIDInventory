using RfidBarcode.Application.Auth.Responses;
using MediatR;

namespace RfidBarcode.Application.Auth.Requests
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public String username { get; set; } = null!;
        public String password { get; set; } = null!;

    }
}
