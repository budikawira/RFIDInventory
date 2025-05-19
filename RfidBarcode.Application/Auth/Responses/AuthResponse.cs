using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using RfidBarcode.Application.Common;
using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Auth.Responses
{
    public class AuthResponse : BaseResponse
    {
        public long? UserId { get; set; } = null!;
        public string? Username { get; set; }
        public string? JwtToken { get; set; }

    }
}
