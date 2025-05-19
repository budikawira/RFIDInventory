using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllItemSummaryForP1Request : BaseDataTableRequest<ItemSummaryForP1VM>, IRequest<BaseDataTableResponse<ItemSummaryForP1VM>>
    {
        public long LocationId { get; set; }
        public GetAllItemSummaryForP1Request(long locationId)
        {
            LocationId = locationId;
        }
    }
}
