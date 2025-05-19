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
    public class GetStockOpnameQuery : BaseHandler, IRequestHandler<GetStockOpnameRequest, BaseObjectResponse<StockOpnameVM>>
    {
        public GetStockOpnameQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<StockOpnameVM>> Handle(GetStockOpnameRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<StockOpnameVM>();

            try
            {
                var query = _context.StockOpnames
                    .Include(x => x.StockOpnameDetails)
                    .Include(x => x.Location)
                    .AsNoTracking().AsQueryable();

                if (request.Data.Id != 0)
                {
                    query = query.Where(x => x.Id == request.Data.Id);
                }
                    var data = await query.FirstOrDefaultAsync();
                if (data != null)
                {
                    response.Data = _mapper.Map<StockOpnameVM>(data);
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
                Console.Error.WriteLine("Exception GetAllItemQuery: " + ex.Message);
            }

            return response;
        }
    }
}
