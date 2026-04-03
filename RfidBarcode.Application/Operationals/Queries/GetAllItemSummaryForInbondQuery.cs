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
    public class GetAllItemSummaryForInbondQuery : BaseHandler, IRequestHandler<GetAllItemSummaryForInbondRequest, BaseDataTableResponse<ItemSummaryForInbondVM>>
    {
        
        public GetAllItemSummaryForInbondQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<ItemSummaryForInbondVM>> Handle(GetAllItemSummaryForInbondRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<ItemSummaryForInbondVM>()
            {
                Draw = request.Draw
            };

            
            try
            {
                var query = _context.Items
                    .Select(x => new
                    {
                        //Kp = (x.Grade != "ALK") ? x.Kp : "",
                        Kode = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode : "",
                        Kode1 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode1 : "",
                        Kode2 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode2 : "",
                        Kode3 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode3 : "",
                        Kode4 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode4 : "",
                        x.Grade,
                        x.LocationId,
                        x.InSuratJalanId,
                        x.OutSuratJalanId
                    })
                    .AsNoTracking()
                    .Where(x => x.InSuratJalanId == null && x.OutSuratJalanId == null)
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
                    .Select(g => new ItemSummaryForInbondVM
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
