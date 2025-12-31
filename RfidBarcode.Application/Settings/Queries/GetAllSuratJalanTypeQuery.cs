using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Settings.Queries
{
    public class GetAllSuratJalanTypeQuery : BaseHandler, IRequestHandler<GetAllSuratJalanTypeRequest, BaseDataTableResponse<SuratJalanTypeVM>>
    {
        public GetAllSuratJalanTypeQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseDataTableResponse<SuratJalanTypeVM>> Handle(GetAllSuratJalanTypeRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseDataTableResponse<SuratJalanTypeVM>()
            {
                Draw = request.Draw
            };


            try
            {
                var query = _context.SuratJalanTypes
                    .AsNoTracking()
                    .Select(a => new SuratJalanTypeVM
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Type = a.Type,
                    }).AsQueryable();

                var total = query.Count();
                var totalFiltered = total;

                if (request.Data != null)
                {
                    if (!string.IsNullOrEmpty(request.Data.Type))
                    {
                        query = query.Where(x => x.Type.StartsWith(request.Data.Type));
                    }

                    totalFiltered = await query.CountAsync();
                }
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    var search = request.SearchValue.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(search) || 
                        (x.Type != null && x.Type.ToLower().Contains(search)));
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
                Console.Error.WriteLine($"Exception {this.GetType().Name.ToString()}: " + ex.Message);
            }
            return response;
        }
    }
}
