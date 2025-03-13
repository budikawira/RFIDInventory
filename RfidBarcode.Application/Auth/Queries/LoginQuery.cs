using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using RfidBarcode.Application.Auth.Requests;
using RfidBarcode.Application.Auth.Responses;
using RfidBarcode.Application.Common;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace RfidBarcode.Application.Auth.Queries
{
    public class LoginQuery : BaseHandler, IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly AppConfig _appConfig; 

        public LoginQuery(IApplicationDbContext context, IMapper mapper,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config, IOptions<AppConfig> appConfig)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _config = config;
            _appConfig = appConfig.Value;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = new LoginResponse();
            try
            {
                var result = await _signInManager.PasswordSignInAsync(request.username, request.password,
                false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    response.Message = "Login successful!";
                    response.Result = BaseResponse.RESULT_OK;
                    
                }
                else
                {
                    response.Message = "Invalid user or password";
                }
            } catch (Exception ex) {
                
                    response.Message = "Exception : " + ex.Message;
            }
            


            return response;
        }
    }
}
