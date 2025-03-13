using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
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
using Newtonsoft.Json;

namespace RfidBarcode.Application.Auth.Queries
{
    public class RefreshTokenQuery : BaseHandler, IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly IConfiguration _config;
        public RefreshTokenQuery(IApplicationDbContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new RefreshTokenResponse();
            try
            {

                response.Token = Helper.GenerateJSONWebToken(_config, new UserVM() { Id = request.UserId}, request.DeviceId);
                response.Message = "Login successful!";
                response.Result = BaseResponse.RESULT_OK;
            } 
            catch (Exception ex) 
            {
                response.Message = "Exception : " + ex.Message;
            }
            
            return response;
        }
        
    }
}
