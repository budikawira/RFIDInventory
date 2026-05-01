using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Queries;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Crm.Areas.Settings.Pages.Users;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Crm.Pages.Finish
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public ItemVM Item { get; set; }

        private readonly IMediator _mediator;
        private readonly IUserResolverService _user;
        public UpdateModel(IMediator mediator, IUserResolverService user)
        {
            _mediator = mediator;
            _user = user;

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
            if (!_user.HasReadAccess(AccessMenu.InputBarcode))
            {
                return new OkObjectResult(new BaseResponse() { Message = "NotAuthorized!" });
            }
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

        public async Task<IActionResult> OnPostReprintAsync(long id)
        {
            var cmd = new DeleteItemPrintLogRequest(id);
            var res = await _mediator.Send(cmd);
            return new OkObjectResult(res);
        }
    }
}
