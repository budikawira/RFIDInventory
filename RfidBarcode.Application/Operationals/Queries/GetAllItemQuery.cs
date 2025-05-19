using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;
using System.Globalization;
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
                    .Include(x => x.SuratJalanP1)
                    .AsNoTracking()
                    .Select(item => new ItemVM
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
                        LocationName = item.Location != null ? item.Location.Name : "",
                        LocationType = item.Location != null ? item.Location.Type : null,
                        Epc = item.Epc,
                        SuratJalanP1 = item.SuratJalanP1 != null ? item.SuratJalanP1.No : null
                    }).AsQueryable();


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

                    if (request.Data.SuratJalanP1Id != null)
                    {
                        query = query.Where(x => x.SuratJalanP1Id == request.Data.SuratJalanP1Id);
                    }

                    if (request.Data.LocationId != null)
                    {
                        query = query.Where(x => x.LocationId == request.Data.LocationId);
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
                

                if (request.ExcludedSuratJalanP1Id != null)
                {
                    var p1 = await _context.SuratJalanP1s.Where(x => x.Id == request.ExcludedSuratJalanP1Id).FirstOrDefaultAsync();
                    if (p1 != null)
                    {
                        if (p1.Grade == "ALK")
                        {
                            query = query.Where(x =>
                                x.Grade == p1.Grade
                                );
                        }
                        else
                        {
                            query = query.Where(x =>
                                //x.Kp == k3.Kp && 
                                x.Kode1 == p1.Kode1 &&
                                x.Kode2 == p1.Kode2 &&
                                x.Kode3 == p1.Kode3 &&
                                x.Kode4 == p1.Kode4 &&
                                x.Grade == p1.Grade
                                );
                        }
                    }
                    query = query.Where(x => x.SuratJalanP1Id == null)
                        .Where(x => x.LocationType == Domain.Entities.Location.TYPE_END_LOCATION);
                }


                //additional queries for Handheld Find
                DateTime createdDate;
                if (DateTime.TryParseExact(request.TanggalProduksi, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out createdDate))
                {
                    query = query.Where(x => (x.TanggalBuatBarcode != null &&
                        x.TanggalBuatBarcode.Value.Date == createdDate.Date));
                }
                if (!string.IsNullOrEmpty(request.Kode))
                {
                    query = query.Where(x => x.Kode != null &&
                        x.Kode.ToLower().Contains(request.Kode.ToLower()));
                }
                //end addtional queries

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
                Console.Error.WriteLine("Exception GetAllItemQuery: " + ex.Message);
            }

            return response;
        }
    }
}
