using DocumentFormat.OpenXml.Vml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Reports.Requests;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Crm.Areas.Reports.Pages
{
    public class StockModel : PageModel
    {
        private readonly IMediator _mediator;

        public StockModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnPostRefreshTableAsync(string? kode, string? grade, string? start, string? end)
        {
            var result = new List<DailySummaryVM>();
            var startDate = Helper.ParseDate(start);
            if (startDate == null) startDate = DateTime.MinValue;
            var endDate = Helper.ParseDate(end);
            if (endDate == null) endDate = DateTime.MaxValue;
            if (startDate != null && endDate != null)
            {
                var cmd = new CreateDailySummaryRequest(startDate.Value, endDate.Value)
                {
                    Kode = kode,
                    Grade = grade
                };
                var res = await _mediator.Send(cmd);

                if (res.Result == Application.Common.BaseObjects.BaseResponse.RESULT_OK && res.Data != null)
                {
                    foreach (var key in res.Data.Keys)
                    {
                        result.AddRange(res.Data[key]);
                    }
                }
            }

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDownloadAsync(string? kode, string? grade, string? start, string? end)
        {
            var startDate = Helper.ParseDate(start);
            if (startDate == null) startDate = DateTime.MinValue;
            var endDate = Helper.ParseDate(end);
            if (endDate == null) endDate = DateTime.MaxValue;
            if (startDate != null && endDate != null)
            {
                var cmd = new CreateDailySummaryRequest(startDate.Value, endDate.Value)
                {
                    Kode = kode,
                    Grade = grade
                };
                var res = await _mediator.Send(cmd);
                
                if (res.Result == Application.Common.BaseObjects.BaseResponse.RESULT_OK && res.Data != null)
                {
                    var wb = Helper.CreateExcelDailyReport(res.Data);
                    if (wb != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            var content = stream.ToArray();
                            var fileName = $"Laporan_Stock_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        }
                    }
                }
            }

            return StatusCode(500);
        }
    }
}
