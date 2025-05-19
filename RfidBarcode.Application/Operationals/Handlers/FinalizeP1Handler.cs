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
    public class FinalizeP1Handler : BaseHandler, IRequestHandler<FinalizeP1Request, BaseResponse>
    {
        public FinalizeP1Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(FinalizeP1Request request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                //validate number items to ensure that it can be printed
                var items = await _context.Items.Where(x => x.SuratJalanP1Id == request.SuratJalanP1Id)
                    .Select(item => new ItemVM()
                    {
                        Id = item.Id,
                        Merk = item.Merk,
                        Kp = item.Kp,
                        Kode = item.Kode,
                        Kode1 = item.Kode1,
                        Kode2 = item.Kode2,
                        Kode3 = item.Kode3,
                        Kode4 = item.Kode4,
                        Oz = item.Oz,
                        Grade = item.Grade,
                        Point = item.Point,
                        Yard = item.Yard,
                        Kg = item.Kg,
                        Lebar = item.Lebar,
                        K = item.K,
                        SusutLusi = item.SusutLusi,
                        SerialNumber = item.SerialNumber,
                        K3l = item.K3l,
                        Inisial = item.Inisial,
                        UserId = item.UserId,
                        SuratJalanP1Id = item.SuratJalanP1Id,
                        QcFinishUserId = item.QcFinishUserId,
                        QcFinish = item.QcFinish,
                        TanggalBuatBarcode = item.TanggalBuatBarcode,
                        ScanP1UserId = item.ScanP1UserId,
                        ScanP1 = item.ScanP1,
                        TrackingItemId = item.TrackingItemId,
                        CreatedDate = item.CreatedDate ?? DateTime.MinValue,
                        CreatedBy = item.CreatedBy,
                        LastUpdateBy = item.LastUpdateBy,
                        LastUpdateDate = item.LastUpdateDate ?? DateTime.MinValue,
                        PrintCount = item.ItemPrintLogs.Count,
                        LocationId = item.LocationId,
                        LocationName = item.Location != null ? item.Location.Name : "",
                        LocationType = item.Location != null ? item.Location.Type : null,
                        Epc = item.Epc,
                        SuratJalanP1 = item.SuratJalanP1 != null ? item.SuratJalanP1.No : null
                    })
                    .ToListAsync();
                if (!Helper.ValidateSuratJalanColumns(items))
                {
                    response.Message = "Jumlah Item melebihi kolom Surat Jalan";
                    return response;
                }

                //check if any duplicate no
                if (await _context.SuratJalanP1s.Where(x => x.No == request.No).AnyAsync())
                {
                    response.Message = "Nomor Surat Jalan sudah pernah digunakan!";
                    return response;
                }

                var k3 = await _context.SuratJalanP1s.Where(x => x.Id == request.SuratJalanP1Id).FirstOrDefaultAsync();
                if (k3 != null)
                {
                    k3.Type = request.Type;
                    k3.No = request.No;
                    k3.FinalizeDate = DateTime.Now;

                    await _context.SaveChangesAsync(cancellationToken);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Data berhasil difinalisasi!";

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
