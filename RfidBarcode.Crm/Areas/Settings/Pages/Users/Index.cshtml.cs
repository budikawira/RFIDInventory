using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Users.Queries;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RfidBarcode.Crm.Areas.Settings.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public List<RoleVM> Roles { get; set; }

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
            Roles = new List<RoleVM>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var cmd = new GetAllRoleRequest();
            var data = await _mediator.Send(cmd);
            if (data.Data != null)
            {
                Roles = data.Data;
            }
            return Page();
        }



        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var response = new BaseDataTableResponse<UserVM>();
            try
            {
                var request = new GetAllUserRequest();
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostSaveAsync(UserVM data)
        {
            var response = new BaseObjectResponse<UserVM>();
            try
            {
                var request = new CreateUserRequest(data);
                response = await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var response = new BaseResponse();
            try
            {
                var request = new DeleteUserRequest(id);
                response = await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return new OkObjectResult(response);
        }
    }
}
