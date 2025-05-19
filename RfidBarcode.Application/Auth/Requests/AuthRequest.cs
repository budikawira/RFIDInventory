using RfidBarcode.Application.Auth.Responses;
using MediatR;

namespace RfidBarcode.Application.Auth.Requests
{
    public class AuthRequest : IRequest<AuthResponse>
    {
        public String Username { get; set; } = null!;
        public String Password { get; set; } = null!;

    }
}
