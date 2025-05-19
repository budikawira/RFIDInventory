using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Operationals.Responses
{
    public class StockOpnameUploadResponse : BaseResponse
    {
        public List<string> OkTagIds { get; set; } //TagId
        public Dictionary<string, string> NokTagIds { get; set; }
        //index: TagId
        //content: Error message

        public StockOpnameUploadResponse()
        {
            OkTagIds = new List<string>();
            NokTagIds = new Dictionary<string, string>();
        }
    }
}
