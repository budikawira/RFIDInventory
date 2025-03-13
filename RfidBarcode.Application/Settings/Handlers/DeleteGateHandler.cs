using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Settings.Handlers
{
    public class DeleteGateHandler : BaseHandler, IRequestHandler<DeleteGateRequest, BaseResponse>
    {
        public DeleteGateHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteGateRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                
                var entity = await _context.Gates.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
                if (entity != null)
                {
                    _context.Gates.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                    response.Message = "Berhasil hapus data!";
                    response.Result = BaseResponse.RESULT_OK;
                }
                else
                {
                    response.Message = "Data tidak ditemukan";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return response;
        }
    }
}
