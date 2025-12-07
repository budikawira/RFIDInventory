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
    public class CreateSuratJalanTypeHandler : BaseHandler, IRequestHandler<CreateSuratJalanTypeRequest, BaseObjectResponse<SuratJalanTypeVM>>
    {
        public CreateSuratJalanTypeHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<SuratJalanTypeVM>> Handle(CreateSuratJalanTypeRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<SuratJalanTypeVM>();

            try
            {
                if (request.Data.Id == 0)
                {
                    var entity = _mapper.Map<SuratJalanType>(request.Data);

                    await _context.SuratJalanTypes.AddAsync(entity);
                    await _context.SaveChangesAsync(cancellationToken);

                    response.Data = _mapper.Map<SuratJalanTypeVM>(entity);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Berhasil tambah data!";
                }
                else
                {
                    var entity = await _context.SuratJalanTypes.Where(x => x.Id == request.Data.Id).FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        entity.Name = request.Data.Name;
                        entity.Type = request.Data.Type;

                        await _context.SaveChangesAsync(cancellationToken);
                        response.Data = _mapper.Map<SuratJalanTypeVM>(entity);
                        response.Message = "Berhasil ubah data!";
                        response.Result = BaseResponse.RESULT_OK;
                    }
                    else
                    {
                        response.Message = "Data tidak ditemukan";
                    }
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
