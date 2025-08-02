using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Application.Reports.Requests
{
    public class CreateDailySummaryRequest : IRequest<BaseObjectResponse<Dictionary<string, List<DailySummaryVM>>>>
    {
        public DateTime PreviousDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public CreateDailySummaryRequest(DateTime previousDate, DateTime currentDate)
        {
            PreviousDate = previousDate;
            CurrentDate = currentDate;
        }
    }
}
