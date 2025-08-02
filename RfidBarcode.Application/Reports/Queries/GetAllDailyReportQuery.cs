using AutoMapper;
using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Reports.Requests;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Vml;
using RfidBarcode.Application.Reports.ViewModels;

namespace RfidBarcode.Application.Reports.Queries
{
    public class GetAllDailyReportQuery : BaseHandler, IRequestHandler<GetAllDailyReportRequest, 
        BaseDataTableResponse<DailyReportVM>>
    {
        public GetAllDailyReportQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<DailyReportVM>> Handle(GetAllDailyReportRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<DailyReportVM>()
            {
                Draw = request.Draw
            };

            try
            {
                var query = _context.DailyReports
                    .Select(x => new DailyReportVM
                    {
                        Id = x.Id,
                        CurrentDate = x.CurrentDate,
                        PreviousDate = x.PreviousDate,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate ?? DateTime.MinValue,
                    })
                    .AsQueryable();

                var total = query.Count();
                var totalFiltered = total;


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
                Console.Error.WriteLine("Exception GetAllItemCutLogQuery: " + ex.Message);
            }

            return response;
        }
    }
}
