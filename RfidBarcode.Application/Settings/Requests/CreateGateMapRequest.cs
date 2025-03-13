using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class CreateGateMapRequest : BaseObjectRequest<GateMapVM>, IRequest<BaseObjectResponse<GateMapVM>>
    {
        public CreateGateMapRequest(GateMapVM data) : base(data)
        {
        }
    }
}
