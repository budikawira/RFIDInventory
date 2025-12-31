using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class GetLocationRequest : BaseObjectRequest<LocationVM>, IRequest<BaseObjectResponse<LocationVM>>
    {

        public GetLocationRequest(LocationVM data) : base(data)
        {
        }
    }
}
