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
    public class CreateGateHandler : BaseHandler, IRequestHandler<CreateGateRequest, BaseObjectResponse<GateVM>>
    {
        private readonly IMqttClientService _mqtt;
        public CreateGateHandler(IApplicationDbContext context, IMapper mapper, IMqttClientService mqtt)
        {
            _context = context;
            _mapper = mapper;
            _mqtt = mqtt;
        }

        public async Task<BaseObjectResponse<GateVM>> Handle(CreateGateRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<GateVM>();

            try
            {
                if (request.Data.Id == 0)
                {
                    var entity = _mapper.Map<Gate>(request.Data);

                    await _context.Gates.AddAsync(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                    response.Data = _mapper.Map<GateVM>(entity);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Berhasil tambah data!";
                }
                else
                {
                    var entity = await _context.Gates.Where(x => x.Id == request.Data.Id).FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        entity.Name = request.Data.Name;
                        entity.ClientId = request.Data.ClientId;

                        await _context.SaveChangesAsync(cancellationToken);
                        response.Data = _mapper.Map<GateVM>(entity);
                        response.Message = "Berhasil ubah data!";
                        response.Result = BaseResponse.RESULT_OK;
                    }
                    else
                    {
                        response.Message = "Data tidak ditemukan";
                    }
                }
                await _mqtt.RestartAsync();
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return response;
        }
    }
}
