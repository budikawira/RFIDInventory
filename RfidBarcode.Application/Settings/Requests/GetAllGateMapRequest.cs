using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class GetAllGateMapRequest : BaseDataTableRequest<GateMapVM>, IRequest<BaseDataTableResponse<GateMapVM>>
    {
    }
}
