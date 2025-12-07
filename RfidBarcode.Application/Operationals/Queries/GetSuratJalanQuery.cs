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
    public class GetSuratJalanQuery : BaseHandler, IRequestHandler<GetSuratJalanRequest, BaseObjectResponse<SuratJalanVM>>
    {
        public GetSuratJalanQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<SuratJalanVM>> Handle(GetSuratJalanRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<SuratJalanVM>();

            try
            {
                var query = _context.SuratJalans
                    .AsNoTracking().AsQueryable();

                if (request.Data.Id != 0)
                {
                    query = query.Where(x => x.Id == request.Data.Id);
                }

                var data = await query.FirstOrDefaultAsync();
                if (data != null)
                {
                    response.Data = _mapper.Map<SuratJalanVM>(data);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "";
                }
                else
                {
                    response.Message = "Data tidak ditemukan!";
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception GetSuratJalanP1Query: " + ex.Message);
            }

            return response;
        }
    }
}
