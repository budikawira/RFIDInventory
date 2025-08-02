using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Application.Reports.Requests
{
    public class GetAllDailyReportRequest : BaseDataTableRequest<DailyReportVM>, 
        IRequest<BaseDataTableResponse<DailyReportVM>>
    {
    }
}
