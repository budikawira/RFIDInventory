using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllItemSummaryForOutbondRequest : BaseDataTableRequest<ItemSummaryForOutbondVM>, IRequest<BaseDataTableResponse<ItemSummaryForOutbondVM>>
    {
        public long LocationId { get; set; }
        public GetAllItemSummaryForOutbondRequest(long locationId)
        {
            LocationId = locationId;
        }
    }
}
