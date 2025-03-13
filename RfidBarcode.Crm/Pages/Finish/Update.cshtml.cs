using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.Queries;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Crm.Pages.Finish
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public ItemVM Item { get; set; }

        private readonly IMediator _mediator;

        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;

            Item = new ItemVM();
        }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id != 0)
            {
                var cmd = new GetItemRequest(new ItemVM { Id = id });
                var res = await _mediator.Send(cmd);
                if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
                {
                    Item = res.Data;
                }
            }
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cmd = new CreateItemRequest(Item);
            var res = await _mediator.Send(cmd);
            if (res.Result == BaseResponse.RESULT_OK)
            {
                TempData["Success"] = res.Message;
            }
            else
            {
                TempData["Error"] = res.Message;
            }
            return Redirect("/Finish/Update?id=" + Item.Id);
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var response = new BaseDataTableResponse<ItemVM>();
            try
            {
                var request = new GetAllItemRequest();
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }
    }
}
