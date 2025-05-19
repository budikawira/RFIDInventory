using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.Responses;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class StockOpnameUploadRequest : IRequest<StockOpnameUploadResponse>
    {
        public long LocationId { get; set; }

        public long UserId { get; set; }

        public List<string> TagIds { get; set; } = null!;
        public List<string> Misplaced { get; set; } = null!;
        public List<string> NotScanned { get; set; } = null!;

        public StockOpnameUploadRequest()
        {
            TagIds = new List<string>();
            Misplaced = new List<string>();
            NotScanned = new List<string>();
        }
    }
}
