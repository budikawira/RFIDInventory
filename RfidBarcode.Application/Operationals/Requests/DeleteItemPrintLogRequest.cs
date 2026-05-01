using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class DeleteItemPrintLogRequest : IRequest<BaseResponse>
    {
        public long ItemId { get; set; }

        public DeleteItemPrintLogRequest(long itemId) 
        {
            ItemId = itemId;
        }
    }
}
