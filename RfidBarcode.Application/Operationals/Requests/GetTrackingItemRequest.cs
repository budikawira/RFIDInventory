using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetTrackingItemRequest : BaseObjectRequest<TrackingItemVM>, IRequest<BaseObjectResponse<TrackingItemVM>>
    {
        public GetTrackingItemRequest(TrackingItemVM data) : base(data) { }
    }
}
