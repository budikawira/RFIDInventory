using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllTrackingItemRequest : BaseDataTableRequest<TrackingItemVM>, IRequest<BaseDataTableResponse<TrackingItemVM>>
    {
        public int Mode { get; set; }

        public const int MODE_ALL = 0;
        public const int MODE_NOT_PROCESS = 1;
        public const int MODEL_ON_PROCESS = 2;
        public const int MODEL_COMPLETED = 3;

        public GetAllTrackingItemRequest(int mode)
        {
            Mode = mode;
        }
    }
}
