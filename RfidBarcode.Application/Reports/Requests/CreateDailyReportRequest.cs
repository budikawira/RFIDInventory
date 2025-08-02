using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Application.Reports.Requests
{
    public class CreateDailyReportRequest : BaseObjectRequest<DailyReportVM>,
        IRequest<BaseResponse>
    {
        public CreateDailyReportRequest(DailyReportVM data) : base(data)
        {
        }
    }
}
