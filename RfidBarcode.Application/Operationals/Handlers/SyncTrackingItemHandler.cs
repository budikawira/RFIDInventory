using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Services;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class SyncTrackingItemHandler : BaseHandler, IRequestHandler<SyncTrackingItemRequest, BaseResponse>
    {
        private readonly IConfiguration _config;
        public SyncTrackingItemHandler(IApplicationDbContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public async Task<BaseResponse> Handle(SyncTrackingItemRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            var httpClientService = new HttpClientService();

            try
            {
                var url = _config["StaticJwt:Url"]?.ToString() ?? "";

                var trackingItem = await _context.TrackingItems.OrderByDescending(x => x.ImportTime).FirstOrDefaultAsync();
                if (trackingItem != null && trackingItem.ImportTime != null)
                {
                    request.Start = trackingItem.ImportTime.Value;
                }
                var res = await httpClientService.RequestTrackingItemAsync(
                    url, request, Helper.GenerateStaticJSONWebToken(_config));
                if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
                {
                    foreach (var vm in res.Data)
                    {
                        vm.ImportTime = vm.LastUpdateDate;
                        var entity = _mapper.Map<TrackingItem>(vm);
                        await _context.TrackingItems.AddAsync(entity);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Berhasil sync data!";
                }
            }
            catch (HttpRequestException ex)
            {
                response.Message = "Request Failed! " + ex.Message;
            }

            return response;
        }
    }
}
