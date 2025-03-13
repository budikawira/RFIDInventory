using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.Queries;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Crm.Common.ViewModels;

namespace RfidBarcode.Crm.Areas.Settings.Pages.Gates
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public GateVM ViewModel { get; set; } = null!;

        private readonly IMediator _mediator;

        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
            ViewModel = new GateVM();
        }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id > 0)
            {
                var cmd = new GetGateRequest(new GateVM() { Id = id });
                var res = await _mediator.Send(cmd);
                if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
                {
                    ViewModel = res.Data;
                }
                else
                {
                    TempData["Error"] = "Invalid data!";
                    return Redirect("/Settings/Gates/");
                }
            }
            

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cmd = new CreateGateRequest(ViewModel);
            var res = await _mediator.Send(cmd);
            if (res.Result == BaseResponse.RESULT_OK)
            {
                TempData["Success"] = res.Message;
            }
            else
            {
                TempData["Error"] = res.Message;
            }


            return Redirect("/Settings/Gates/Update?id=" + ViewModel.Id);
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var response = new BaseDataTableResponse<GateMapVM>();
            try
            {
                var request = new GetAllGateMapRequest();
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

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

        public async Task<IActionResult> OnPostSaveAsync(GateMapVM data)
        {
            var response = new BaseObjectResponse<GateMapVM>();
            try
            {
                var request = new CreateGateMapRequest(data);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

    }
}
