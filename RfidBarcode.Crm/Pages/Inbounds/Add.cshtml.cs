using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Crm.Common;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Crm.Pages.Inbounds
{
    public class AddModel : BasePageModel
    {
        [BindProperty]
        public string? Kp { get; set; }

        [BindProperty]
        public string? Kode { get; set; }

        [BindProperty]
        public string? Kode1 { get; set; }

        [BindProperty]
        public string? Kode2 { get; set; }

        [BindProperty]
        public string? Kode3 { get; set; }

        [BindProperty]
        public string? Kode4 { get; set; }

        [BindProperty]
        public string? Grade { get; set; }

        public AddModel(IMediator mediator, IUserResolverService user) : base(mediator)
        {
            HasAccess = user.HasReadAccess(AccessMenu.SuratJalanInbound);
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var request = new GetAllItemSummaryForInbondRequest();
            request.InitFromDataTable(Request.Form);

            var response = await _mediator.Send(request);

            return new OkObjectResult(response);
        }


        public async Task<IActionResult> OnPostCreateInbondAsync()
        {
            var response = new BaseResponse();

            var cmd = new CreateSuratJalanRequest();
            cmd.Kode = Kode;
            cmd.Kode1 = Kode1;
            cmd.Kode2 = Kode2;
            cmd.Kode3 = Kode3;
            cmd.Kode4 = Kode4;
            cmd.Grade = Grade;
            cmd.SuratJalanType = SuratJalanType.TYPE_INBOUND;
            response = await _mediator.Send(cmd);

            return new OkObjectResult(response);
        }
    }
}
