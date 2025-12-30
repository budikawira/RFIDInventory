using AutoMapper;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
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
    public class ConfirmSuratJalanHandler : BaseHandler, IRequestHandler<ConfirmSuratJalanRequest, BaseResponse>
    {
        private IUserResolverService _user;

        public ConfirmSuratJalanHandler(IApplicationDbContext context, IMapper mapper, IUserResolverService user)
        {
            _context = context;
            _mapper = mapper;
            _user = user;
        }

        public async Task<BaseResponse> Handle(ConfirmSuratJalanRequest request, CancellationToken cancellationToken)
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

                var qry = _context.Items.AsQueryable();
                var items = new List<Item>();
                if (srtJalan.SuratJalanType == SuratJalanType.TYPE_INBOUND)
                {
                    qry = qry.Where(x => x.InSuratJalanId == request.SuratJalanId);

                    //validate number items to ensure that it can be printed
                    items = await qry
                        .ToListAsync();
                    var invalidItems = items.Where(x => x.LocationId == null).Select(x => x.Id).ToList();

                    if (invalidItems.Count > 0)
                    {
                        response.Message = "Terdapat item yang belum dipindahkan ke lokasi tujuan, Tag Id: " + String.Join(", ", invalidItems);
                        return response;
                    }


                    srtJalan.ConfirmDate = DateTime.Now;
                    foreach (var item in items)
                    {
                        item.InScan = srtJalan.ConfirmDate;
                        item.InScanUserId = _user.GetUserId();
                        item.InScanUser = _user.GetUser();
                    }
                }
                else
                {
                    qry = qry.Where(x => x.OutSuratJalanId == request.SuratJalanId);
                    items = await qry
                        .Include(x => x.Location)
                        .ToListAsync();
                    var invalidItems = items.Where(x =>
                    x.Location == null || x.Location.Type != Location.TYPE_END_LOCATION).Select(x => x.Id).ToList();

                    if (invalidItems.Count > 0)
                    {
                        response.Message = "Terdapat item yang belum dikeluarkan, Tag Id: " + String.Join(", ", invalidItems);
                        return response;
                    }


                    srtJalan.ConfirmDate = DateTime.Now;
                    foreach (var item in items)
                    {
                        item.OutScan = srtJalan.ConfirmDate;
                        item.OutScanUserId = _user.GetUserId();
                        item.OutScanUser = _user.GetUser();
                    }
                }


                await _context.SaveChangesAsync(cancellationToken);
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Data berhasil difinalisasi!";
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return response;
        }
    }
}
