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
    public class GetLocationQuery : BaseHandler, IRequestHandler<GetLocationRequest, BaseObjectResponse<LocationVM>>
    {
        public GetLocationQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<LocationVM>> Handle(GetLocationRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<LocationVM>();


            try
            {
                var query = _context.Locations
                    .AsNoTracking()
                    .Select(a => new LocationVM
                    {
                        Id = a.Id,
                        Name = a.Name
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
