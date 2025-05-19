using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.Common;
using ZXing.ImageSharp;
using static System.Net.Mime.MediaTypeNames;

namespace RfidBarcode.Crm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        [HttpGet]
        public IActionResult GenerateBarcode(
        [FromQuery] string text = "1234567890",
        [FromQuery] int width = 250,
        [FromQuery] int height = 250,
        [FromQuery] BarcodeFormat format = BarcodeFormat.QR_CODE)
        {
            try
            {
                /// Create barcode writer
                var writer = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
                {
                    Format = format,
                    Options = new EncodingOptions
                    {
                        Height = height,
                        Width = width,
                        Margin = 1,
                        PureBarcode = true // Remove text below barcode
                    }
                };

                // Generate barcode directly to memory stream
                var memoryStream = new MemoryStream();
                using (var image = writer.Write(text))
                {
                    image.SaveAsPng(memoryStream);
                }

                memoryStream.Position = 0;
                return File(memoryStream, "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating barcode: {ex.Message}");
            }
        }
    }
}
