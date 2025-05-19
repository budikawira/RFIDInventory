using AutoMapper;
using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Reports.Requests;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Vml;

namespace RfidBarcode.Application.Reports.Queries
{
    public class GetAllReceivedItemQuery : BaseHandler, IRequestHandler<GetAllReceivedItemRequest, BaseDataTableResponse<ItemVM>>
    {
        public GetAllReceivedItemQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<ItemVM>> Handle(GetAllReceivedItemRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<ItemVM>()
            {
                Draw = request.Draw
            };

            try
            {
                var query = _context.ItemMovements
                    .Include(x => x.Item)
                        .ThenInclude(x => x.ItemPrintLogs)
                    .Include(x => x.Item)
                        .ThenInclude(x => x.Location)
                    .Include(x => x.Item)
                        .ThenInclude(x => x.SuratJalanP1)
                    .AsNoTracking()
                    .Where(x => (x.CreatedDate != null && 
                        x.CreatedDate.Value.Date >= request.StartDate && 
                        x.CreatedDate.Value.Date <= request.EndDate) && 
                        x.LocationId == request.LocationId && 
                        x.Item != null
                    )
                    .Select(x => new ItemVM
                    {
                        Id = x.Item!.Id,
                        Merk = x.Item!.Merk,
                        Kp = x.Item!.Kp,
                        Kode = x.Item!.Kode,
                        Kode1 = x.Item!.Kode1,
                        Kode2 = x.Item!.Kode2,
                        Kode3 = x.Item!.Kode3,
                        Kode4 = x.Item!.Kode4,
                        Oz = x.Item!.Oz,
                        Grade = x.Item!.Grade,
                        Point = x.Item!.Point,
                        Yard = x.Item!.Yard,
                        Kg = x.Item!.Kg,
                        Lebar = x.Item!.Lebar,
                        K = x.Item!.K,
                        SusutLusi = x.Item!.SusutLusi,
                        SerialNumber = x.Item!.SerialNumber,
                        K3l = x.Item!.K3l,
                        Inisial = x.Item!.Inisial,
                        UserId = x.Item!.UserId,
                        QcFinishUserId = x.Item!.QcFinishUserId,
                        QcFinish = x.Item!.QcFinish,
                        TanggalBuatBarcode = x.Item!.TanggalBuatBarcode,
                        SuratJalanP1Id = x.Item!.SuratJalanP1Id,
                        ScanP1UserId = x.Item!.ScanP1UserId,
                        ScanP1 = x.Item!.ScanP1,
                        TrackingItemId = x.Item!.TrackingItemId,
                        CreatedDate = x.CreatedDate ?? DateTime.MinValue,
                        CreatedBy = x.Source,
                        LastUpdateBy = x.Source,
                        LastUpdateDate = x.Item!.LastUpdateDate ?? DateTime.MinValue,
                        PrintCount = x.Item!.ItemPrintLogs.Count,
                        LocationId = x.Item!.LocationId,
                        LocationName = x.Item!.Location != null ? x.Item!.Location.Name : "",
                        LocationType = x.Item!.Location != null ? x.Item!.Location.Type : null,
                        Epc = x.Item!.Epc,
                        SuratJalanP1 = x.Item!.SuratJalanP1 != null ? x.Item!.SuratJalanP1.No : null
                }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;

                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();

                    query = query.Where(x => x.Merk.ToLower().Contains(search) || x.Kp.ToLower().Contains(search));

                    totalFiltered = await query.CountAsync();
                }

                if (!string.IsNullOrEmpty(request.SortColumn) && !string.IsNullOrEmpty(request.SortColumn))
                {
                    query = query.OrderBy(request.SortColumn + " " + request.SortColumnDir);
                }
                query = query.Skip(request.Skip);

                if (request.PageSize > 0)
                {
                    query = query.Take(request.PageSize);
                }

                response.Data = await query.ToListAsync();
                response.RecordsTotal = total;
                response.RecordsFiltered = totalFiltered;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception GetAllReceivedItemQuery: " + ex.Message);
            }

            return response;
        }
    }
}
