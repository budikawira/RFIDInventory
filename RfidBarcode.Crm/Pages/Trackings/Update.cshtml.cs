using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Crm.Pages.Trackings
{
    public class UpdateModel : PageModel
    {
        public TrackingItemVM TrackingItem { get; set; }

        private readonly IMediator _mediator;
        
        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
            TrackingItem = new TrackingItemVM();
        }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var cmd = new GetTrackingItemRequest(new TrackingItemVM { Id = id });
            var res = await _mediator.Send(cmd);
            if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
            {
                TrackingItem = res.Data;
                return Page();
            }
            return Redirect("/Trackings/");
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var temp = Request.Form["trackingItemId"].FirstOrDefault();
            long trackingItemId;
            long.TryParse(temp, out trackingItemId);
            var response = new BaseDataTableResponse<ItemVM>();
            try
            {
                var request = new GetAllItemRequest()
                {
                    Data = new ItemVM() { TrackingItemId = trackingItemId }
                };
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostSaveAsync(ItemVM data)
        {
            var response = new BaseObjectResponse<ItemVM>();

            if (data.TrackingItemId != null)
            {
                var cmd1 = new GetTrackingItemRequest(new TrackingItemVM { Id = (long)data.TrackingItemId! });
                var res1 = await _mediator.Send(cmd1);
                if (res1.Result == BaseResponse.RESULT_OK && res1.Data != null)
                {
                    data.Merk = res1.Data.Merk ?? "";
                    data.Kp = res1.Data.Kp ?? ""; // Or: data.Kp = res1.Data.Kp; if Kp should not be empty

                    data.Kode = res1.Data.Kode;
                    data.Kode1 = res1.Data.Kode1;
                    data.Kode2 = res1.Data.Kode2;
                    data.Kode3 = res1.Data.Kode3;
                    data.Kode4 = res1.Data.Kode4;
                    data.Oz = res1.Data.Oz;
                    data.Grade = res1.Data.Grade;
                    data.Point = res1.Data.Point;

                    var cmd = new CreateItemRequest(data);
                    response  =await _mediator.Send(cmd);
                }
            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostFinalizeAsync(long id)
        {
            var response = new BaseResponse();

            var cmd1 = new GetTrackingItemRequest(new TrackingItemVM { Id = id });
            var res1 = await _mediator.Send(cmd1);
            if (res1.Result == BaseResponse.RESULT_OK && res1.Data != null)
            {
                res1.Data.EndProcess = DateTime.Now;
                var cmd = new CreateTrackingItemRequest(res1.Data);
                response = await _mediator.Send(cmd);
            }

            
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostResetStatusAsync(long id)
        {
            var response = new BaseResponse();

            var cmd1 = new GetTrackingItemRequest(new TrackingItemVM { Id = id });
            var res1 = await _mediator.Send(cmd1);
            if (res1.Result == BaseResponse.RESULT_OK && res1.Data != null)
            {
                res1.Data.EndProcess = null;
                var cmd = new CreateTrackingItemRequest(res1.Data);
                response = await _mediator.Send(cmd);
            }


            return new OkObjectResult(response);
        }


        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var response = new BaseResponse();
            try
            {
                var request = new DeleteItemRequest(id);
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
