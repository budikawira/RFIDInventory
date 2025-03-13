using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class GetGateRequest : BaseObjectRequest<GateVM>, IRequest<BaseObjectResponse<GateVM>>
    {
        public GetGateRequest(GateVM data) : base(data)
        {
        }
    }
}
