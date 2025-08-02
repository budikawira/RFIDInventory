using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Reports.Requests
{
    public class GetAllStockSummaryRequest : BaseDataTableRequest<ItemVM>, IRequest<BaseDataTableResponse<ItemVM>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long LocationId { get; set; }

        public GetAllStockSummaryRequest(DateTime startDate, DateTime endDate, long locationId)
        {
            StartDate = startDate;
            EndDate = endDate;
            LocationId = locationId;
        }
    }
}
