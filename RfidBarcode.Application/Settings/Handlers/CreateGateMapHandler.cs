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
    public class CreateGateMapHandler : BaseHandler, IRequestHandler<CreateGateMapRequest, BaseObjectResponse<GateMapVM>>
    {
        public CreateGateMapHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<GateMapVM>> Handle(CreateGateMapRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<GateMapVM>();

            try
            {
                if (request.Data.Id == 0)
                {
                    var entity = _mapper.Map<GateMap>(request.Data);

                    await _context.GateMaps.AddAsync(entity);
                    await _context.SaveChangesAsync(cancellationToken);

                    var data = await _context.GateMaps
                        .Include(x => x.Gate)
                        .Include(x => x.NextLocation)
                        .Include(x => x.PrevLocation)
                        .Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                    response.Data = _mapper.Map<GateMapVM>(data);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Berhasil tambah data!";
                }
                else
                {
                    var entity = await _context.GateMaps.Where(x => x.Id == request.Data.Id).FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        entity.Antenna = request.Data.Antenna;
                        entity.PrevLocationId= request.Data.PrevLocationId;
                        entity.NextLocationId = request.Data.NextLocationId;

                        await _context.SaveChangesAsync(cancellationToken);
                        
                        var data = await _context.GateMaps
                            .Include(x => x.Gate)
                            .Include(x => x.NextLocation)
                            .Include(x => x.PrevLocation)
                            .Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                        response.Data = _mapper.Map<GateMapVM>(data);
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
