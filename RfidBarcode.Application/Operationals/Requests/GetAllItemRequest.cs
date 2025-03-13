using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllItemRequest : BaseDataTableRequest<ItemVM>, IRequest<BaseDataTableResponse<ItemVM>>
    {
        public List<long>? Ids { get; set; }
        public string? PrintStatus { get; set; }
        public long? LocationId { get; set; }
    }
}
