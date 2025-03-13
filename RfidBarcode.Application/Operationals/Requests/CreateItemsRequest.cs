using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class CreateItemsRequest : BaseObjectRequest<List<ItemVM>>, IRequest<BaseResponse>
    {
        public CreateItemsRequest(List<ItemVM> data) : base(data) { }
    }
}
