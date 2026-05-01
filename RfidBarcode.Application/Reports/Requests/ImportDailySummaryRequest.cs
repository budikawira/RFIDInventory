using MediatR;
using Microsoft.AspNetCore.Http;
using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Reports.Requests
{
    public class ImportDailySummaryRequest : IRequest<BaseResponse>
    {
        public IFormFile File { get; set; }
        
        public ImportDailySummaryRequest(IFormFile file)
        {
            File = file;
        }
    }
}