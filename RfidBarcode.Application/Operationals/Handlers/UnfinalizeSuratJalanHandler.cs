using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class UnfinalizeSuratJalanHandler : BaseHandler, IRequestHandler<UnfinalizeSuratJalanRequest, BaseResponse>
    {
        public UnfinalizeSuratJalanHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(UnfinalizeSuratJalanRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var srtJalan = await _context.SuratJalans.Where(x => x.Id == request.SuratJalanId).FirstOrDefaultAsync();
                if (srtJalan == null)
                {
                    response.Message = "Data tidak ditemukan";
                    return response;
                }

                srtJalan.FinalizeDate = null;

                await _context.SaveChangesAsync(cancellationToken);
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Finalisasi data berhasil dibatalkan!";
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return response;
        }
    }
}
