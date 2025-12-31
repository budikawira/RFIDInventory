using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
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
    public class UpdateItemLocationsHandler : BaseHandler, IRequestHandler<UpdateItemLocationsRequest, BaseResponse>
    {
        public UpdateItemLocationsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(UpdateItemLocationsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var items = await _context.Items
                    .Include(x => x.Location)
                    .Where(x => request.ItemIds.Contains(x.Id))
                    .ToListAsync();

                var suratJalan = await _context.SuratJalans
                    .FirstOrDefaultAsync(x => x.Id == request.SuratJalanId);

                if (suratJalan == null)
                {
                    response.Message = "Surat Jalan not found.";
                    return response;
                }

                var newLocationName = "";
                if (request.NewLocationId != null)
                {
                    var newLocation = await _context.Locations
                        .FirstOrDefaultAsync(x => x.Id == request.NewLocationId);
                    if (newLocation == null)
                    {
                        response.Message = "Surat Jalan not found.";
                        return response;
                    }
                    newLocationName = newLocation.Name;
                }

                foreach (var item in items)
                {
                    item.LocationId = request.NewLocationId;
                    var itemMovement = new ItemMovement
                    {
                        ItemId = item.Id,
                        PrevLocationId = item.LocationId,
                        LocationId = request.NewLocationId,
                        PrevLocationName = item.Location != null ? item.Location.Name : "",
                        LocationName = newLocationName,
                        Note = $"Updated via Surat Jalan {suratJalan.No}",
                        Source = ItemMovement.SOURCE_UPDATE
                    };
                    await _context.ItemMovements.AddAsync(itemMovement);
                }
                    
                await _context.SaveChangesAsync(cancellationToken);
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Lokasi Finish berhasil diubah!";
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return response;
        }
    }
}
