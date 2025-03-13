
namespace RfidBarcode.Application.Common.BaseObjects
{
    public class BaseResponse
    {
        public int Result { get; set; }
        public string? Message { get; set; }

        public const int RESULT_OK = 0;
        public const int RESULT_NOK = 1;

        public BaseResponse()
        {
            Result = RESULT_NOK;
            Message = "Generic error";
        }

    }
}
