using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Exports;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Reports.Requests;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using System.Threading.Tasks;

namespace RfidBarcode.Crm.Areas.Reports.Pages
{
    public class ReceiveModel : PageModel
    {
        private readonly IMediator _mediator;
        public List<LocationVM> Locations { get; set; }

        public ReceiveModel(IMediator mediator)
        {
            _mediator = mediator;
            Locations = new List<LocationVM>();
        }

        public async Task OnGet()
        {
            var cmdL = new GetAllLocationRequest()
            {
                SortColumn = "Name",
                SortColumnDir = "asc",
                PageSize = 100
            };
            var resL = await _mediator.Send(cmdL);
            if (resL.Result == BaseResponse.RESULT_OK && resL.Data != null)
            {
                Locations = resL.Data;
            }

        }

        public async Task<IActionResult> OnPostRefreshDataAsync(string startDate, string endDate, long locationId)
        {
            var response = new BaseDataTableResponse<ItemVM>();
            var dateStart = Helper.ParseDate(startDate);
            var dateEnd = Helper.ParseDate(endDate);
            if (dateStart != null && dateEnd != null)
            {
                var cmd = new GetAllReceivedItemRequest(dateStart.Value, dateEnd.Value, locationId);
                cmd.InitFromDataTable(Request.Form);
                response = await _mediator.Send(cmd);
            }
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostDownloadAsync(string startDate, string endDate, long locationId)
        {
            var res = new BaseDataTableResponse<ItemVM>();
            var dateStart = Helper.ParseDate(startDate);
            var dateEnd = Helper.ParseDate(endDate);
            if (dateStart != null && dateEnd != null)
            {
                var location = "";
                var cmdL = new GetLocationRequest(new LocationVM() { Id = locationId });
                var resL = await _mediator.Send(cmdL);
                if (resL.Result == BaseResponse.RESULT_OK && resL.Data != null)
                {
                    location = resL.Data.Name;
                }
                var cmd = new GetAllReceivedItemRequest(dateStart.Value, dateEnd.Value, locationId);
                res = await _mediator.Send(cmd);

                if (res.Data == null)
                {
                    return StatusCode(500, res.Message ?? "");
                }

                var export = new ExportExcel<ItemVM>("Penerimaan",
                ["Lokasi: " + location, "Tanggal Penerimaan: " + dateStart.Value.ToString("yyyy-MM-dd") + 
                    " s/d " + dateEnd.Value.ToString("yyyy-MM-dd")],
                ["Tanggal Terima",
                    "Tag Id",
                    "Merk",
                    "KP",
                    "Kode 1",
                    "Kode 2",
                    "Kode 3",
                    "Kode 4",
                    "OZ",
                    "Grade",
                    "Point",
                    "Yard",
                    "KG",
                    "Lebar",
                    "K",
                    "Susut Lusi",
                    "Serial Number",
                    "K3L",
                    "Inisial",
                    "Tanggal Buat Barcode",
                    "Srt Jln K3"
                    ],
                ["CreatedDateString",
                    "Id",
                    "Merk",
                    "Kp",
                    "Kode1",
                    "Kode2",
                    "Kode3",
                    "Kode4",
                    "Oz",
                    "Grade",
                    "Point",
                    "Yard",
                    "Kg",
                    "Lebar",
                    "K",
                    "SusutLusi",
                    "SerialNumber",
                    "K3l",
                    "Inisial",
                    "TanggalBuatBarcodeString",
                    "SuratJalanP1"
                    ],
                [
                    ExportExcel<ItemVM>.TYPE_STRING, //CreatedDateString
                    ExportExcel<ItemVM>.TYPE_LONG, //Id
                    ExportExcel<ItemVM>.TYPE_STRING, //Merk
                    ExportExcel<ItemVM>.TYPE_STRING, //Kp
                    ExportExcel<ItemVM>.TYPE_STRING, //Kode1
                    ExportExcel<ItemVM>.TYPE_STRING, //Kode2
                    ExportExcel<ItemVM>.TYPE_STRING, //Kode3
                    ExportExcel<ItemVM>.TYPE_STRING, //Kode4
                    ExportExcel<ItemVM>.TYPE_STRING, //Oz
                    ExportExcel<ItemVM>.TYPE_STRING, //Grade
                    ExportExcel<ItemVM>.TYPE_DECIMAL, //point
                    ExportExcel<ItemVM>.TYPE_DECIMAL, //yard
                    ExportExcel<ItemVM>.TYPE_DECIMAL, //kg
                    ExportExcel<ItemVM>.TYPE_STRING, //lebar
                    ExportExcel<ItemVM>.TYPE_STRING, //k
                    ExportExcel<ItemVM>.TYPE_STRING, //SusutLusi
                    ExportExcel<ItemVM>.TYPE_STRING, //SerialNumber
                    ExportExcel<ItemVM>.TYPE_STRING, //K3l
                    ExportExcel<ItemVM>.TYPE_STRING, //Inisial
                    ExportExcel<ItemVM>.TYPE_STRING, //TangggalBuatBarcodeString
                    ExportExcel<ItemVM>.TYPE_STRING, //SuratJalanP1
                ]);
                using (XLWorkbook wb = export.ExportFile(res.Data, typeof(ItemVM).GetProperties()))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "Download.xlsx");
                    }
                }

            }
            return StatusCode(500, res.Message ?? "");
        }
    }
}
