namespace RfidBarcode.Application.Common.BaseObjects
{
    public class BaseObjectResponse<T> : BaseResponse
    {
        public T? Data { get; set; }
    }
}
