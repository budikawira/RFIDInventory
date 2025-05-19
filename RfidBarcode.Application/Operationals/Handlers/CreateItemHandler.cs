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
    public class CreateItemHandler : BaseHandler, IRequestHandler<CreateItemRequest, BaseObjectResponse<ItemVM>>
    {
        public CreateItemHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<ItemVM>> Handle(CreateItemRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<ItemVM>();

            try
            {
                if (request.Data.Id == 0)
                {
                    var entity = _mapper.Map<Item>(request.Data);
                    entity.Kode = "" + request.Data.Kode1;
                    entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode2)) ? " " : "";
                    entity.Kode += request.Data.Kode2;
                    entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode3)) ? " " : "";
                    entity.Kode += request.Data.Kode3;
                    entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode4)) ? " " : "";
                    entity.Kode += request.Data.Kode4;
                    entity.K3l = Helper.GetK3L(entity.Kode3 ?? "");
                    entity.Location = null;
                    await _context.Items.AddAsync(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                    entity.Epc = Helper.GetEpc(entity.Id);
                    entity.Qr = Helper.GetQr(entity);
                    await _context.SaveChangesAsync(cancellationToken);

                    response.Data = _mapper.Map<ItemVM>(request.Data);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Berhasil tambah data!";
                }
                else
                {
                    var entity = await _context.Items.Include(x => x.Location)
                        .Where(x => x.Id == request.Data.Id).FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        if (request.Data.TanggalBuatBarcode == null || request.Data.Yard == null || request.Data.Kg == null || request.Data.K == null)
                        {
                            response.Message = "Invalid input parameter";
                            return response;
                        }

                        entity.Merk = request.Data.Merk;
                        entity.Kp = request.Data.Kp;
                        entity.Kode1 = request.Data.Kode1;
                        entity.Kode2 = request.Data.Kode2;
                        entity.Kode3 = request.Data.Kode3;
                        entity.Kode4 = request.Data.Kode4;

                        entity.Kode = "" + request.Data.Kode1;
                        entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode2)) ? " " : "";
                        entity.Kode += request.Data.Kode2;
                        entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode3)) ? " " : "";
                        entity.Kode += request.Data.Kode3;
                        entity.Kode += (!string.IsNullOrEmpty(entity.Kode) && !string.IsNullOrEmpty(request.Data.Kode4)) ? " " : "";
                        entity.Kode += request.Data.Kode4;

                        entity.K3l = Helper.GetK3L(entity.Kode3 ?? "");
                        entity.Oz = request.Data.Oz;
                        entity.Grade = request.Data.Grade;
                        entity.Point = request.Data.Point;
                        entity.Yard = request.Data.Yard;
                        entity.Kg = request.Data.Kg;
                        entity.Lebar = request.Data.Lebar;
                        entity.K = request.Data.K;
                        entity.SusutLusi = request.Data.SusutLusi;
                        entity.SerialNumber = request.Data.SerialNumber;
                        entity.K3l = request.Data.K3l;
                        entity.Inisial = request.Data.Inisial;
                        entity.UserId = request.Data.UserId;
                        entity.SuratJalanP1Id = request.Data.SuratJalanP1Id;
                        entity.QcFinishUserId = request.Data.QcFinishUserId;
                        entity.QcFinish = request.Data.QcFinish;
                        entity.TanggalBuatBarcode = request.Data.TanggalBuatBarcode.Value;
                        entity.SuratJalanP1Id = request.Data.SuratJalanP1Id;
                        entity.ScanP1UserId = request.Data.ScanP1UserId;
                        entity.ScanP1 = request.Data.ScanP1;
                        entity.Epc = Helper.GetEpc(entity.Id);
                        entity.Qr = Helper.GetQr(entity);
                        entity.LocationId = request.Data.LocationId;
                        await _context.SaveChangesAsync(cancellationToken);
                        response.Data = _mapper.Map<ItemVM>(entity);
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
