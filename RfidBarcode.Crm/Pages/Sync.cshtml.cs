using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Infrastructure.Services;

namespace RfidBarcode.Crm.Pages
{
    public class SyncModel : PageModel
    {
        private readonly IMediator _mediator;
        public SyncModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> OnGetAsync()
        {

            var cmd = new SyncTrackingItemRequest(100);
            var res = await _mediator.Send(cmd);
            return Page();
        }
    }
}
