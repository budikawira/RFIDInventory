using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class CreateItemPrintLogsHandler : BaseHandler, IRequestHandler<CreateItemPrintLogsRequest, BaseResponse>
    {
        public CreateItemPrintLogsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(CreateItemPrintLogsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                foreach (var itemId in request.Data)
                {
                    await _context.ItemPrintLogs.AddAsync(new ItemPrintLog() { ItemId = itemId });
                }
                await _context.SaveChangesAsync(cancellationToken);
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Create print log successfull!";
            }
            catch (Exception ex)
            {
                response.Message = "Fail creating print log: " + ex.Message;
            }

            return response;
        }
    }
}
