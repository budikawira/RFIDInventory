using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Exports;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Crm.Common.ViewModels;
using SQLitePCL;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace RfidBarcode.Crm.Pages.Finish
{
    public class IndexModel : PageModel
    {
        public class RowData
        {
            public int Index { get; set; }
            public long Id { get; set; }
        }

        [BindProperty]
        public List<RowData>? Ids { get; set; }

        private readonly IMediator _mediator;
        private readonly IUserResolverService _user;
        public IndexModel(IMediator mediator, IUserResolverService user)
        {
            _mediator = mediator;
            _user = user;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var printStatus = Request.Form["printStatus"].ToString();
            var tempLocation = Request.Form["locationId"];
            var temp = Request.Form["tagId"];
            var response = new BaseDataTableResponse<ItemVM>();
            try
            {
                var request = new GetAllItemRequest() { PrintStatus = printStatus};

                if (!string.IsNullOrEmpty(temp))
                {
                    var array = Regex.Split(temp!, @"\s*,\s*")
                          .Where(s => !string.IsNullOrWhiteSpace(s))
                          .ToArray();
                    if (array.Length > 0)
                    {
                        request.Ids = new List<long>();
                        foreach (var row in array)
                        {
                            long tagId;
                            if (long.TryParse(row, out tagId))
                            {
                                request.Ids.Add(tagId);
                            }
                        }
                    }
                }

                long locationId;
                if (long.TryParse(tempLocation, out locationId))
                {
                    request.LocationId = locationId;
                }
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var response = new BaseResponse();
            try
            {
                var request = new DeleteItemRequest(id);
                response = await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return new OkObjectResult(response);
        }


        public async Task<IActionResult> OnPostRefreshLocationAsync()
        {
            var response = new BaseObjectResponse<List<ItemVM>>();

            var param = new List<long>();

            if (Ids != null)
            {
                foreach (var row in Ids)
                {
                    param.Add(row.Id);
                }

                var cmd = new GetAllItemRequest()
                {
                    Ids = param
                };
                var res = await _mediator.Send(cmd);
                if (res.Data != null)
                {
                    response.Data = res.Data;
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "";
                }
            }
            
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostImportAsync()
        {
            var response = new BaseResponse();
            var indexColumn = new string[]
            {
                "Merk", "Kp", "Kode1", "Kode2", "Kode3", "Kode4",
                "Oz", "Grade", "Point", "Yard", "Kg", "Lebar", "K", 
                "SusutLusi", "SerialNumber", "Inisial", "TanggalBuatBarcode"

            };

            var errorList = new List<string>();

            IFormFile file = Request.Form.Files[0];
            try
            {
                using (var workbook = new XLWorkbook(file.OpenReadStream()))
                {
                    var ws = workbook.Worksheet(1);
                    var rowCount = ws.LastRowUsed()?.RowNumber();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                        
                            var json = new JsonObject();
                            for (int col = 0; col < indexColumn.Length; col++)
                            {
                                json[indexColumn[col]] = ws.Cell(row, col + 1).GetValue<string>() ?? "";
                            }

                            ItemVM? itemVM = JsonConvert.DeserializeObject<ItemVM>(json.ToJsonString());
                            if (itemVM == null)
                            {
                                errorList.Add("Error line-" + row + " : Data is null");
                            }
                            else
                            {
                                itemVM.UserId = _user.GetUserId();
                                itemVM.LocationId = null;
                                var cmd = new CreateItemRequest(itemVM);
                                var res = await _mediator.Send(cmd);
                                if (res.Result != BaseResponse.RESULT_OK)
                                {
                                    errorList.Add("Error line-" + row + " : " + res.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorList.Add("Error line-" + row + " : " + ex.Message);
                        }
                    }

                    if (errorList.Count == 0)
                    {
                        response.Result = BaseResponse.RESULT_OK;
                        response.Message = "Berhasil import data!";
                    }
                    else
                    {
                        response.Message = string.Join("\n", errorList);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception: " + ex.Message;
            }



            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostLocationAsync()
        {
            List<Select2Item> data = new List<Select2Item>();
            string? search = Request.Form["term"].FirstOrDefault();
            var cmd = new GetAllLocationRequest()
            {
                SearchValue = search,
                PageSize = 100
            };
            var res = await _mediator.Send(cmd);
            if (res.Data != null)
            {
                foreach (var item in res.Data)
                {
                    data.Add(new Select2Item(item.Name, item.Id.ToString()));
                }
            }
            return new OkObjectResult(data);
        }

        public async Task<IActionResult> OnPostPrintAsync(long[] ids)
        {
            var response = new BaseResponse();
            try
            {
                var cmd = new GetAllItemRequest()
                {
                    Ids = new List<long>(ids)
                };

                var res = await _mediator.Send(cmd);

                if (res.Data == null)
                {
                    return StatusCode(500, res.Message ?? "");
                }

                var export = new ExportExcel<ItemVM>("",
                [],
                ["DESCRIPTION", "PLU", "KP", "CODE1", "CODE2", "CODE3", "CODE4", "GRADE", "LOT", "POINT", 
                    "QTY (YD)", "WEIGHT (KG)", 
                    "SL", "LEBAR", "INISIAL", "K", "K3L", "EPC"
                    ],
                ["Merk", "SerialNumber", "Kp", "Kode1", "Kode2", "Kode3", "Kode4", "Grade", "Lot", "Point", 
                    "Yard", "Kg", 
                    "SusutLusi", "Lebar", "Inisial", "K", "K3l", "TagId"
                    ],
                [
                    ExportExcel<ItemVM>.TYPE_STRING, //merk
                    ExportExcel<ItemVM>.TYPE_STRING, //serialnumber
                    ExportExcel<ItemVM>.TYPE_STRING, //kp
                    ExportExcel<ItemVM>.TYPE_STRING, //kode1
                    ExportExcel<ItemVM>.TYPE_STRING, //kode2
                    ExportExcel<ItemVM>.TYPE_STRING, //kode3
                    ExportExcel<ItemVM>.TYPE_STRING, //kode4
                    ExportExcel<ItemVM>.TYPE_STRING, //grade
                    ExportExcel<ItemVM>.TYPE_STRING, //lot
                    ExportExcel<ItemVM>.TYPE_STRING, //point
                    ExportExcel<ItemVM>.TYPE_DECIMAL, //yard
                    ExportExcel<ItemVM>.TYPE_DECIMAL, //kg
                    ExportExcel<ItemVM>.TYPE_STRING, //susutlusi
                    ExportExcel<ItemVM>.TYPE_STRING, //lebar
                    ExportExcel<ItemVM>.TYPE_STRING, //inisial
                    ExportExcel<ItemVM>.TYPE_STRING, //k
                    ExportExcel<ItemVM>.TYPE_STRING, //k3l
                    ExportExcel<ItemVM>.TYPE_STRING, //tagid
                ]);

                using (XLWorkbook wb = export.ExportFile(res.Data, typeof(ItemVM).GetProperties()))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        var cmdLog = new CreateItemPrintLogsRequest(new List<long>(ids));
                        var resLog = await _mediator.Send(cmdLog);
                        if (resLog.Result == BaseResponse.RESULT_OK)
                        {

                            return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Print.xlsx");
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return new OkObjectResult(response);
        }
    }
}
