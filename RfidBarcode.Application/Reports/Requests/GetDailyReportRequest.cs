using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Application.Reports.Requests
{
    public class GetDailyReportRequest : 
        IRequest<BaseObjectResponse<DailyReportVM>>
    {
        public long Id { get; set; }
        public GetDailyReportRequest(long id)
        {
            Id = id;
        }
    }
}
