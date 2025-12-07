using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Crm.Pages.SuratJalanP1s
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public long LocationId { get; set; }

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

        public List<LocationVM> Locations { get; set; }

        private readonly IMediator _mediator;

        public AddModel(IMediator mediator)
        {
            _mediator = mediator;
            Locations = new List<LocationVM>();
        }

        public async Task OnGetAsync()
        {
            var cmdL = new GetAllLocationRequest() { IsForSummaryK3 = true};
            var resL = await _mediator.Send(cmdL);
            Locations = resL.Data;
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var request = new GetAllItemSummaryForOutbondRequest(LocationId);
            request.InitFromDataTable(Request.Form);

            var response = await _mediator.Send(request);

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostCreateP1Async()
        {
            var response = new BaseResponse();

            var cmd = new CreateSuratJalanRequest();
            cmd.Kode = Kode;
            cmd.Kode1 = Kode1;
            cmd.Kode2 = Kode2;
            cmd.Kode3 = Kode3;
            cmd.Kode4 = Kode4;
            cmd.Grade = Grade;
            cmd.SuratJalanType = SuratJalanType.TYPE_OUTBOND;
            response = await _mediator.Send(cmd);

            return new OkObjectResult(response);
        }
    }
}
