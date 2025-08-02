using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Reports.Requests;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Crm.Areas.Reports.Pages
{
    public class DailyReportModel : PageModel
    {
        private readonly IMediator _mediator;
        public DailyReportModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var response = new BaseDataTableResponse<DailyReportVM>();

            try
            {
                var request = new GetAllDailyReportRequest();
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnGetDownloadAsync(long id)
        {
            var cmd = new GetDailyReportRequest(id);
            var res = await _mediator.Send(cmd);

            // Convert the file content to a byte array
            if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
            {
                return File(res.Data.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    res.Data.GetFileName() + ".xlsx");
            }
            return NotFound("Report not found or an error occurred.");

        }
        
    }
}
