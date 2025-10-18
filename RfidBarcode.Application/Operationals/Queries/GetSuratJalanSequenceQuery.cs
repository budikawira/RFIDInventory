using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Operationals.Queries
{
    public class GetSuratJalanSequenceQuery : IRequestHandler<GetSuratJalanSequenceRequest, BaseObjectResponse<Int32>>
    {
        private readonly IApplicationDbContext _context;

        public GetSuratJalanSequenceQuery(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseObjectResponse<int>> Handle(GetSuratJalanSequenceRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<int>();
            try
            {
                var noPrefix = $"{request.Type}/{request.Code}";
                var count = await _context.SuratJalanP1s
                    .Where(sj => sj.No != null && sj.No.StartsWith(noPrefix))
                    .OrderByDescending(sj => sj.Sequence)
                    .Select(sj => sj.Sequence)
                    .FirstOrDefaultAsync();


                response.Data = count + 1;
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "ok";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
