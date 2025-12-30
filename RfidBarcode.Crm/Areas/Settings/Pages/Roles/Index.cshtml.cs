using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Crm.Common;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Crm.Areas.Settings.Pages.Roles
{
    [Authorize(Policy = AccessMenu.RoleManagement)]
    public class IndexModel : BasePageModel
    {
        public IndexModel(IMediator mediator, IUserResolverService user) : base(mediator)
        {
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var response = new BaseDataTableResponse<RoleVM>();
            try
            {
                var request = new GetAllRoleRequest();
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostAccRefreshDataAsync(int roleId)
        {
            var response = new BaseDataTableResponse<AccessMenuRoleVM>();
            try
            {
                var request = new GetAllAccessMenuRoleRequest(roleId);
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostAccDeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var request = new DeleteAccessMenuRoleRequest(id);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostModalRefreshDataAsync(int excludedRoleId)
        {
            var response = new BaseDataTableResponse<AccessMenuVM>();
            try
            {
                var request = new GetAllAccessMenuRequest()
                {
                    ExcludedRoleId = excludedRoleId
                };
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostAddAccItemsAsync(long roleId, List<string> accIds)
        {
            var cmd = new CreateAccessMenuRoleRequest(roleId, accIds);
            var response = await _mediator.Send(cmd);

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostSaveAsync(RoleVM data)
        {
            var response = new BaseObjectResponse<RoleVM>();
            var command = new CreateRoleRequest(data);

            response = await _mediator.Send(command);


            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var response = new BaseResponse();
            try
            {
                var request = new DeleteRoleRequest(id);
                response = await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return new OkObjectResult(response);
        }
    }
}
