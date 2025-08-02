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
    public class GetDailyReportQuery : BaseHandler, IRequestHandler<GetDailyReportRequest, 
        BaseObjectResponse<DailyReportVM>>
    {
        public GetDailyReportQuery(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseObjectResponse<DailyReportVM>> Handle(GetDailyReportRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<DailyReportVM>();

            try
            {
                var data = await _context.DailyReports
                    .Select(x => new DailyReportVM
                    {
                        Id = x.Id,
                        Content = x.Content,
                        CurrentDate = x.CurrentDate,
                        PreviousDate = x.PreviousDate,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate ?? DateTime.MinValue,
                    })
                    .Where(x => x.Id == request.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (data == null)
                {
                    response.Message = "No data found for the given ID.";
                    return response;
                }

                if (response.Result == BaseResponse.RESULT_OK && data != null)
                {
                    response.Data = data;
                }
                else
                {
                    response.Message = "Failed to retrieve data.";
                }
                response.Data = data;
                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Data retrieved successfully.";
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception {this.GetType().Name}: {ex.Message}");
            }

            return response;
        }
    }
}
