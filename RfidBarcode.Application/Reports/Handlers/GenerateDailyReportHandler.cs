
using AutoMapper;
using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Reports.Requests;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Reports.Handlers
{
    public class GenerateDailyReportHandler : BaseHandler, IRequestHandler<GenerateDailyReportRequest, BaseResponse>
    {
        public GenerateDailyReportHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(GenerateDailyReportRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var entity = _mapper.Map<DailyReport>(request.Data);
                await _context.DailyReports.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
