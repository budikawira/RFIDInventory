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
    public class GetAllItemSummaryForP1Query : BaseHandler, IRequestHandler<GetAllItemSummaryForP1Request, BaseDataTableResponse<ItemSummaryForP1VM>>
    {
        public GetAllItemSummaryForP1Query(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<ItemSummaryForP1VM>> Handle(GetAllItemSummaryForP1Request request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<ItemSummaryForP1VM>()
            {
                Draw = request.Draw
            };

            
            try
            {
                var query = _context.Items
                    .Select(x => new
                    {
                        //Kp = (x.Grade != "ALK") ? x.Kp : "",
                        Kode = (x.Grade != "ALK") ? x.Kode : "",
                        Kode1 = (x.Grade != "ALK") ? x.Kode1 : "",
                        Kode2 = (x.Grade != "ALK") ? x.Kode2 : "",
                        Kode3 = (x.Grade != "ALK") ? x.Kode3 : "",
                        Kode4 = (x.Grade != "ALK") ? x.Kode4 : "",
                        x.Grade,
                        x.LocationId,
                        x.SuratJalanP1Id
                    })
                    .AsNoTracking()
                    .Where(x => x.LocationId == request.LocationId)
                    .Where(x => x.SuratJalanP1Id == null)
                    .GroupBy(x => new
                    {
                        //x.Kp,
                        x.Kode,
                        x.Kode1,
                        x.Kode2,
                        x.Kode3,
                        x.Kode4,
                        x.Grade,
                    })
                    .Select(g => new ItemSummaryForP1VM
                    {
                        //Kp = g.Key.Kp,
                        Kode = g.Key.Kode,
                        Kode1 = g.Key.Kode1,
                        Kode2 = g.Key.Kode2,
                        Kode3 = g.Key.Kode3,
                        Kode4 = g.Key.Kode4,
                        Grade = g.Key.Grade,
                        Count = g.Count()
                    })
                    .AsQueryable();

                var total = query.Count();
                var totalFiltered = total;


                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => //x.Kp.ToLower().Contains(search) ||
                        (x.Kode != null && x.Kode.ToLower().Contains(search)) ||
                        (x.Grade != null && x.Grade.ToLower().Contains(search)));
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
                Console.Error.WriteLine("Exception GetAllItemSummaryForK3Query: " + ex.Message);
            }

            return response;
        }
    }
}
