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
    public class GetItemQuery : BaseHandler, IRequestHandler<GetItemRequest, BaseObjectResponse<ItemVM>>
    {
        public GetItemQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<ItemVM>> Handle(GetItemRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<ItemVM>();

            try
            {
                var query = _context.Items
                    .Include(x => x.ItemPrintLogs)
                    .AsNoTracking().AsQueryable();

                if (request.Data.Id != 0)
                {
                    query = query.Where(x => x.Id == request.Data.Id);
                }
                else if (!string.IsNullOrEmpty(request.Data.Epc))
                {
                    query = query.Where(x => x.Epc != null && x.Epc.ToLower() == request.Data.Epc.ToLower());
                }
                    var data = await query.FirstOrDefaultAsync();
                if (data != null)
                {
                    response.Data = _mapper.Map<ItemVM>(data);
                    response.Data.PrintCount = data.ItemPrintLogs.Count;
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
