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
    public class DeleteItemPrintLogHandler : BaseHandler, IRequestHandler<DeleteItemPrintLogRequest, BaseResponse>
    {
        public DeleteItemPrintLogHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteItemPrintLogRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var entities = await _context.ItemPrintLogs.Where(x => x.ItemId == request.ItemId).ToListAsync(cancellationToken);
                if (entities == null)
                {
                    response.Message = "Data print tidak ditemukan!";
                    return response;
                }

                _context.ItemPrintLogs.RemoveRange(entities);
                await _context.SaveChangesAsync(cancellationToken);
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Berhasil hapus data!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return response;
        }
    }
}
