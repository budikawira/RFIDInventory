

namespace RfidBarcode.Application.Common.BaseObjects
{
    public class BaseObjectRequest<T> where T : class
    {
        public T Data { get; set; } = null!;

        public BaseObjectRequest(T data) { Data = data; }
    }
}
