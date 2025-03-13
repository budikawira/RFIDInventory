using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetItemRequest : BaseObjectRequest<ItemVM>, IRequest<BaseObjectResponse<ItemVM>>
    {
        public GetItemRequest(ItemVM data) : base(data) { }
    }
}
