using RfidBarcode.Application.Auth.Responses;
using RfidBarcode.Application.Common.BaseObjects;
using MediatR;

namespace RfidBarcode.Application.Auth.Requests
{
    public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
    {
        public long UserId { get; set; }
        public string DeviceId { get; set; } = null!;
    }
}
