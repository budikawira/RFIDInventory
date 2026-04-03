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
    public class FinalizeSuratJalanHandler : BaseHandler, IRequestHandler<FinalizeSuratJalanRequest, BaseResponse>
    {
        public FinalizeSuratJalanHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(FinalizeSuratJalanRequest request, CancellationToken cancellationToken)
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
                if (srtJalan.SuratJalanType == SuratJalanType.TYPE_INBOUND)
                {
                    qry = qry.Where(x => x.InSuratJalanId == request.SuratJalanId);
                }
                else
                {
                    qry = qry.Where(x => x.OutSuratJalanId == request.SuratJalanId);
                }

                //validate number items to ensure that it can be printed
                var items = await qry
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
                        OutSuratJalanId = item.OutSuratJalanId,
                        InSuratJalanId = item.InSuratJalanId,
                        QcFinishUserId = item.QcFinishUserId,
                        QcFinish = item.QcFinish,
                        TanggalBuatBarcode = item.TanggalBuatBarcode,
                        OutScanUserId = item.OutScanUserId,
                        OutScan = item.OutScan,
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
                        OutSuratJalan = item.OutSuratJalan != null ? item.OutSuratJalan.No : null
                    })
                    .ToListAsync();
                if (srtJalan.Grade != null && srtJalan.Grade.ToUpper() == "AXP" && !Helper.ValidateSuratJalanColumns(items))
                {
                    response.Message = "Jumlah Item melebihi kolom Surat Jalan";
                    return response;
                }

                if (string.IsNullOrEmpty(srtJalan.No))
                {
                    if (request.Code.Length != 4)
                    {
                        response.Message = "Kode No Surat Jalan harus 4 karakter.";
                        return response;
                    }
                    //this is a new finalization, generate the surat jalan no
                    var no = Helper.GenerateSuratJalanNo(request.Type, request.Code, request.Sequence);

                    //check if any duplicate no
                    if (await _context.SuratJalans.Where(x => x.No == no).AnyAsync())
                    {
                        //try to regenerate with new sequence
                        var noPrefix = $"{request.Type}/{request.Code}";
                        var count = await _context.SuratJalans
                            .Where(sj => sj.No != null && sj.No.StartsWith(noPrefix))
                            .OrderByDescending(sj => sj.Sequence)
                            .Select(sj => sj.Sequence)
                            .FirstOrDefaultAsync();
                        request.Sequence = count + 1;
                        no = Helper.GenerateSuratJalanNo(request.Type, request.Code, request.Sequence);

                        response.Message = "Nomor Surat Jalan sudah pernah digunakan!";
                        return response;
                    }
                    srtJalan.SuratJalanName = request.Type;
                    srtJalan.No = no;
                    srtJalan.Sequence = request.Sequence;
                }

                //check if it is a retur
                var srtJalanType = await _context.SuratJalanTypes.Where(x => x.Name == srtJalan.SuratJalanName).FirstOrDefaultAsync();
                if (srtJalanType != null && srtJalanType.Type == SuratJalanType.TYPE_OUTBOND_RETURN)
                {
                    srtJalan.IsReturn = true;
                }
                srtJalan.FinalizeDate = DateTime.Now;
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
