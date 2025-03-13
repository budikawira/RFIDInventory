using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class CreateItemRequest : BaseObjectRequest<ItemVM>, IRequest<BaseObjectResponse<ItemVM>>
    {
        public CreateItemRequest(ItemVM data) : base(data) { }
    }
}
