using DocumentFormat.OpenXml.Presentation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Crm.Common;
using RfidBarcode.Domain.Entities;
using System.Collections;

namespace RfidBarcode.Crm.Pages.Finish
{
    public class AddModel : BasePageModel
    {
        [BindProperty]
        public List<List<string>> Data { get; set; } = null!;


        public AddModel(IMediator mediator, IUserResolverService user) : base(mediator)
        {
            this.HasAccess = user.HasReadAccess(AccessMenu.InputBarcode);
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            var response = new BaseResponse();

            var rowIndex = 0;
            try
            {
                var listVM = new List<ItemVM>();
                foreach (var row in Data)
                {
                    rowIndex++;
                    if (row.Count != 17)
                    {
                        response.Message = "Error row " + rowIndex;
                        return new OkObjectResult(response);
                    }
                    decimal yard, kg;
                    //decimal point = decimal.Parse(row[8]);
                    yard = decimal.Parse(row[9]);
                    kg = decimal.Parse(row[10]);
                    var vm = new ItemVM();
                    vm.Merk = row[0];
                    vm.Kp = row[1];
                    vm.Kode1 = row[2];
                    vm.Kode2 = row[3];
                    vm.Kode3 = row[4];
                    vm.Kode4 = row[5];
                    vm.Oz = row[6];
                    vm.Grade = row[7];
                    vm.Point = row[8];
                    vm.Yard = yard;
                    vm.Kg = kg;
                    vm.Lebar = row[11];
                    vm.K = row[12];
                    vm.SusutLusi = row[13];
                    vm.SerialNumber = row[14];
                    vm.Inisial = row[15];
                    vm.TanggalBuatBarcode = Helper.ParseDate(row[16]);
                    listVM.Add(vm);
                }
                var cmd = new CreateItemsRequest(listVM);
                response = await _mediator.Send(cmd);
            }
            catch (Exception)
            {
                response.Message = "Invalid row " + rowIndex;
            }
            

            return new OkObjectResult(response);
        }
    }
}
