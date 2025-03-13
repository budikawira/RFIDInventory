using DocumentFormat.OpenXml.Bibliography;
using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class SyncTrackingItemRequest : IRequest<BaseResponse>
    {
        public DateTime Start { get; set; }
        public int MaxCount {  get; set; }
        public SyncTrackingItemRequest(int maxCount) 
        {
            Start = DateTime.MinValue;
            MaxCount = maxCount;
        }
    }
}
