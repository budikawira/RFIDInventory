using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Settings.Queries
{
    public class GetGateQuery : BaseHandler, IRequestHandler<GetGateRequest, BaseObjectResponse<GateVM>>
    {
        public GetGateQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<GateVM>> Handle(GetGateRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<GateVM>();


            try
            {
                var query = _context.Gates
                    .AsNoTracking()
                    .Select(a => new GateVM
                    {
                        Id = a.Id,
                        Name = a.Name,
                        ClientId = a.ClientId,
                    }).AsQueryable();

                if (request.Data != null)
                {
                    if (request.Data.Id != 0)
                    {
                        query = query.Where(x => x.Id == request.Data.Id);
                    }
                }

                response.Data = await query.FirstOrDefaultAsync();
                if (response.Data != null)
                {
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "";
                }
                else
                {
                    response.Message = "Data does not exist!";
                }

            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }
            return response;
        }
    }
}
