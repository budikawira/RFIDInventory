using MediatR;
using Quartz;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Reports.Requests;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Crm.Services
{
    public class DailyReportJob : IJob
    {
        private readonly IMediator _mediator;

        public DailyReportJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var now = DateTime.Now;
            var currentDate = new DateTime(now.Year, now.Month, now.Day);
            var previousDate = currentDate.AddDays(-1);
            var cmd = new CreateDailySummaryRequest(previousDate, currentDate);
            var res = await _mediator.Send(cmd);

            if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
            {
                var wb = Helper.CreateExcelDailyReport(res.Data);
                if (wb != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var f = stream.ToArray();
                        var vm = new DailyReportVM()
                        {
                            Content = f,
                            CurrentDate = currentDate,
                            PreviousDate = previousDate,
                        };
                        var cmdG = new GenerateDailyReportRequest(vm);
                        var resG = await _mediator.Send(cmdG);
                    }
                }
            }
        }
    }
}
