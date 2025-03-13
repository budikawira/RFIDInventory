using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class CreateTrackingItemRequest : BaseObjectRequest<TrackingItemVM>, IRequest<BaseObjectResponse<TrackingItemVM>>
    {
        public CreateTrackingItemRequest(TrackingItemVM data) : base(data) { }
    }
}
