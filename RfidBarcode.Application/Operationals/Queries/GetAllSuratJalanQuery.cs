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
    public class GetAllSuratJalanQuery : BaseHandler, IRequestHandler<GetAllSuratJalanRequest, BaseDataTableResponse<SuratJalanVM>>
    {
        public GetAllSuratJalanQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<SuratJalanVM>> Handle(GetAllSuratJalanRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<SuratJalanVM>()
            {
                Draw = request.Draw
            };

            
            try
            {
                var query = _context.SuratJalans
                    .Include(x => x.Items)
                    .AsNoTracking()
                    .Select(x => new SuratJalanVM
                    {
                        Id = x.Id,
                        CreatedDate = x.CreatedDate ?? DateTime.MinValue,
                        CreatedBy = x.CreatedBy,
                        LastUpdateDate = x.LastUpdateDate ?? DateTime.MinValue,
                        LastUpdateBy = x.LastUpdateBy,
                        SuratJalanType = x.SuratJalanType,
                        SuratJalanName = x.SuratJalanName,
                        No = x.No,
                        //Kp = string.Join(", ", x.Items.OrderBy(x => x.Kp).GroupBy(x => x.Kp).Select(x => x.Key).ToList()),
                        Kode = x.Kode,
                        Kode1 = x.Kode1,
                        Kode2 = x.Kode2,
                        Kode3 = x.Kode3,
                        Kode4 = x.Kode4,
                        Grade = x.Grade,
                        FinalizeDate = x.FinalizeDate,
                        ConfirmDate = x.ConfirmDate,
                    }).AsQueryable();

                var total = await query.CountAsync();
                var totalFiltered = total;

                if (request.Data != null)
                {
                    if (!string.IsNullOrEmpty(request.Data.SuratJalanType))
                    {
                        query = query.Where(x => x.SuratJalanType == request.Data.SuratJalanType);
                    }
                    totalFiltered = await query.CountAsync();
                }

                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => (x.No != null && x.No.ToLower().Contains(search)) ||
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
                Console.Error.WriteLine("Exception GetAllStockOpnameQuery: " + ex.Message);
            }

            return response;
        }
    }
}
