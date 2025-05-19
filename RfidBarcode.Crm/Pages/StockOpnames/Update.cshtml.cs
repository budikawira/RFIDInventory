using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Crm.Pages.StockOpnames
{
    public class UpdateModel : PageModel
    {
        private readonly IMediator _mediator;
        public StockOpnameVM StockOpname { get; set; }
        
        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
            StockOpname = new StockOpnameVM();
        }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var cmd = new GetStockOpnameRequest(new StockOpnameVM() { Id = id });
            var res = await _mediator.Send(cmd);
            if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
            {
                StockOpname = res.Data;
            }
            else
            {
                TempData["Error"] = "Invalid data!";
            }    
            return Page();
        }


        public async Task<IActionResult> OnPostRefreshDataAsync(long stockOpnameId)
        {
            var request = new GetAllStockOpnameDetailRequest() { StockOpnameId = stockOpnameId };
            request.InitFromDataTable(Request.Form);

            var response = await _mediator.Send(request);

            return new OkObjectResult(response);
        }
    }
}
