using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Application.Reports.Requests
{
    public class GenerateDailyReportRequest : BaseObjectRequest<DailyReportVM>,
        IRequest<BaseResponse>
    {
        public GenerateDailyReportRequest(DailyReportVM data) : base(data)
        {
        }
    }
}
