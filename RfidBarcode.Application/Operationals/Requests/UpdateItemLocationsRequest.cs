using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class UpdateItemLocationsRequest : IRequest<BaseResponse>
    {
        public long? SuratJalanId { get; set; }
        public List<long> ItemIds { get; set; }
        public long? NewLocationId { get; set; }
        public UpdateItemLocationsRequest(List<long> itemIds, long? newLocationId) 
        { 
            ItemIds = itemIds;
            NewLocationId = newLocationId;
        }

    }
}
