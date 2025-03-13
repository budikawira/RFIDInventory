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
    public class CreateTrackingItemHandler : BaseHandler, IRequestHandler<CreateTrackingItemRequest, BaseObjectResponse<TrackingItemVM>>
    {
        public CreateTrackingItemHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<TrackingItemVM>> Handle(CreateTrackingItemRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<TrackingItemVM>();

            try
            {
                if (request.Data.Id == 0)
                {
                    var entity = _mapper.Map<TrackingItem>(request.Data);
                    entity.Kode = "" + request.Data.Kode1;
                    entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode2)) ? " " : "";
                    entity.Kode += request.Data.Kode2;
                    entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode3)) ? " " : "";
                    entity.Kode += request.Data.Kode3;
                    entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode4)) ? " " : "";
                    entity.Kode += request.Data.Kode4;
                    await _context.TrackingItems.AddAsync(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                    response.Data = _mapper.Map<TrackingItemVM>(request.Data);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Berhasil tambah data!";
                }
                else
                {
                    var entity = await _context.TrackingItems.Where(x => x.Id == request.Data.Id).FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        //only change the EndProcess
                        entity.EndProcess = request.Data.EndProcess;

                        await _context.SaveChangesAsync(cancellationToken);
                        response.Data = _mapper.Map<TrackingItemVM>(entity);
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
