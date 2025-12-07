using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllItemSummaryForInbondRequest : BaseDataTableRequest<ItemSummaryForInbondVM>, IRequest<BaseDataTableResponse<ItemSummaryForInbondVM>>
    {
        public GetAllItemSummaryForInbondRequest()
        {
        }
    }
}
