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
    public class GetAllTrackingItemQuery : BaseHandler, IRequestHandler<GetAllTrackingItemRequest, BaseDataTableResponse<TrackingItemVM>>
    {
        public GetAllTrackingItemQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<TrackingItemVM>> Handle(GetAllTrackingItemRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<TrackingItemVM>()
            {
                Draw = request.Draw
            };

            
            try
            {
                var query = _context.TrackingItems
                    .Include(x => x.Items)
                    .AsNoTracking()
                    .Select(item => new TrackingItemVM
                    {
                        Id = item.Id,
                        Merk = item.Merk,
                        Kp = item.Kp,
                        Ib = item.Ib,
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
                        SusutLusi = item.SusutLusi,
                        SerialNumber = item.SerialNumber,
                        EndProcess = item.EndProcess,
                        ImportTime = item.ImportTime,
                        ItemCount = item.Items.Count,
                        StockOutDate = item.StockOutDate,
                    }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;

                if (request.Mode == GetAllTrackingItemRequest.MODE_NOT_PROCESS)
                {
                    query = query.Where(x => x.EndProcess == null && x.ItemCount == 0);
                }
                else if (request.Mode == GetAllTrackingItemRequest.MODEL_ON_PROCESS)
                {
                    query = query.Where(x => x.EndProcess == null && x.ItemCount > 0);
                }
                else if (request.Mode == GetAllTrackingItemRequest.MODEL_COMPLETED)
                {
                    query = query.Where(x => x.EndProcess != null);
                }

                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => (x.Merk != null && x.Merk.ToLower().Contains(search)) || 
                        (x.Kp != null && x.Kp.ToLower().Contains(search)));
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
                Console.Error.WriteLine("Exception GetAllTrackingItemQuery: " + ex.Message);
            }

            return response;
        }
    }
}
