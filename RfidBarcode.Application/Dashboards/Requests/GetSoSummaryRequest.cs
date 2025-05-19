using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Dashboards.ViewModels;

namespace RfidBarcode.Application.Dashboards.Requests
{
    public class GetSoSummaryRequest : IRequest<BaseObjectResponse<SoSummaryVM>>
    {
    }
}
