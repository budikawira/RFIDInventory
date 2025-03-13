using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Operationals.Queries
{
    public class GetAllItemQuery : BaseHandler, IRequestHandler<GetAllItemRequest, BaseDataTableResponse<ItemVM>>
    {
        public GetAllItemQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<ItemVM>> Handle(GetAllItemRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<ItemVM>()
            {
                Draw = request.Draw
            };

            
            try
            {
                var query = _context.Items
                    .Include(x => x.ItemPrintLogs)
                    .Include(x => x.Location)
                    .AsNoTracking()
                    .Select(item => new ItemVM
                    {
                        Id = item.Id,
                        Merk = item.Merk,
                        Kp = item.Kp,
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
                        SuratJalanId = item.SuratJalanId,
                        QcFinishUserId = item.QcFinishUserId,
                        QcFinish = item.QcFinish,
                        TanggalBuatBarcode = item.TanggalBuatBarcode,
                        SuratJalanP1Id = item.SuratJalanP1Id,
                        ScanP1UserId = item.ScanP1UserId,
                        ScanP1 = item.ScanP1,
                        TrackingItemId = item.TrackingItemId,
                        CreatedDate = item.CreatedDate ?? DateTime.MinValue,
                        CreatedBy = item.CreatedBy,
                        LastUpdateBy = item.LastUpdateBy,
                        LastUpdateDate = item.LastUpdateDate ?? DateTime.MinValue,
                        PrintCount = item.ItemPrintLogs.Count,
                        LocationId = item.LocationId,
                        LocationName = item.Location != null ? item.Location.Name : ""
                    }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;

                if (request.Ids != null && request.Ids.Count > 0)
                {
                    query = query.Where(x => request.Ids.Contains(x.Id));
                }

                if (request.Data != null)
                {
                    if (request.Data.TrackingItemId != null && request.Data.TrackingItemId != 0)
                    {
                        query = query.Where(x => x.TrackingItemId == request.Data.TrackingItemId);
                    }
                }

                if (!string.IsNullOrEmpty(request.PrintStatus))
                {
                    if (request.PrintStatus == ItemVM.PRINT_STATUS_PENDING)
                    {
                        query = query.Where(x => x.PrintCount == 0);
                    }
                    else if (request.PrintStatus == ItemVM.PRINT_STATUS_DONE)
                    {
                        query = query.Where(x => x.PrintCount > 0);
                    }
                }

                if (request.LocationId != null)
                {
                    query = query.Where(x => x.LocationId == request.LocationId);
                }

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
                Console.Error.WriteLine("Exception GetAllItemQuery: " + ex.Message);
            }

            return response;
        }
    }
}
