using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class RemoveItemsForInbondRequest : IRequest<BaseResponse>
    {
        public List<long> ItemIds { get; set; }
        public RemoveItemsForInbondRequest(List<long> itemIds) 
        {
            ItemIds = itemIds;
        }
    }
}
