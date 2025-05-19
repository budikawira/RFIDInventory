using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Operationals.Responses
{
    public class CreateMultipleSuratJalanP1Response : BaseResponse
    {
        public Dictionary<Int64, string> OkTagIds { get; set; } //TagId
        public Dictionary<Int64, string> NokTagIds { get; set; }
        //index: TagId
        //content: Error message

        public CreateMultipleSuratJalanP1Response()
        {
            OkTagIds = new Dictionary<Int64, string>();
            NokTagIds = new Dictionary<Int64, string>();
        }
    }
}
