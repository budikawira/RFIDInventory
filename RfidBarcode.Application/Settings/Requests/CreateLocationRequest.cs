using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class CreateLocationRequest : BaseObjectRequest<LocationVM>, IRequest<BaseObjectResponse<LocationVM>>
    {
        public CreateLocationRequest(LocationVM data) : base(data)
        {
        }
    }
}
