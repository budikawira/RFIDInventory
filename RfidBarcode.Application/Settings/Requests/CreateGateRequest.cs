using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class CreateGateRequest : BaseObjectRequest<GateVM>, IRequest<BaseObjectResponse<GateVM>>
    {
        public CreateGateRequest(GateVM data) : base(data)
        {
        }
    }
}
