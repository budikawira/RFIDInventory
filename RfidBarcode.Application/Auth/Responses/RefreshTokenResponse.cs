using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Auth.Responses
{
    public class RefreshTokenResponse : BaseResponse
    {
        public string? Token { get; set; }
    }
}
