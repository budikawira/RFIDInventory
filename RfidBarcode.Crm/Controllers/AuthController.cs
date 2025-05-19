using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RfidBarcode.Application.Auth.Requests;
using RfidBarcode.Application.Auth.Responses;
using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Crm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        public async Task<AuthResponse> OnPostLoginAsync(AuthRequest request)
        {
            var response = await _mediator.Send(request);

            return response;
        }

        public BaseResponse Index()
        {
            var response = new BaseResponse();
            return response;
        }
    }
}
