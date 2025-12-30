using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RfidBarcode.Crm.Common
{
    public abstract class BasePageModel : PageModel
    {
        protected bool HasAccess { get; set; } = true;

        protected readonly IMediator _mediator;

        public BasePageModel(IMediator mediator)
        {
            _mediator = mediator;
        }


        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (!HasAccess)
            {
                context.Result = new RedirectResult("/");
            }
        }
    }
}
