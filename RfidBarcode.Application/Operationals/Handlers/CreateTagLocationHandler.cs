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
    public class CreateTagLocationHandler : BaseHandler, IRequestHandler<CreateTagLocationRequest, BaseObjectResponse<TagLocationVM>>
    {
        public CreateTagLocationHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<TagLocationVM>> Handle(CreateTagLocationRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<TagLocationVM>();

            try
            {
                if (request.Data.Id == 0)
                {
                    var entity = _mapper.Map<TagLocation>(request.Data);
                    await _context.TagLocations.AddAsync(entity);
                    await _context.SaveChangesAsync(cancellationToken);

                    response.Data = _mapper.Map<TagLocationVM>(entity);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Berhasil tambah data!";
                }
                else
                {
                    var entity = await _context.TagLocations.Where(x => x.Id == request.Data.Id).FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        entity.Epc = request.Data.Epc;
                        entity.ItemId = request.Data.ItemId;
                        entity.StartScanned = request.Data.StartScanned;
                        entity.EndScanned = request.Data.EndScanned;
                        entity.LastScanned = request.Data.LastScanned;
                        entity.LocationId = request.Data.LocationId;

                        await _context.SaveChangesAsync(cancellationToken);
                        response.Data = _mapper.Map<TagLocationVM>(entity);
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
