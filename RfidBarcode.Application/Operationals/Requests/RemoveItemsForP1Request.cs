using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class RemoveItemsForP1Request : IRequest<BaseResponse>
    {
        public List<long> ItemIds { get; set; }
        public RemoveItemsForP1Request(List<long> itemIds) 
        {
            ItemIds = itemIds;
        }
    }
}
