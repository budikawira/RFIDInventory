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
    public class DeleteStockOpnameHandler : BaseHandler, IRequestHandler<DeleteStockOpnameRequest, BaseResponse>
    {
        public DeleteStockOpnameHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteStockOpnameRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var entity = await _context.StockOpnames.FindAsync(request.Id);
                if (entity == null)
                {
                    response.Message = "Data tidak ditemukan!";
                    return response;
                }

                _context.StockOpnames.Remove(entity);
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
