using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using RfidBarcode.Application.Common;
using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Auth.Responses
{
    public class LoginResponse : BaseResponse
    {
        public string? Token { get; set; }

    }
}
