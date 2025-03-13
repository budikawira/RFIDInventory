using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class GetAllLocationRequest : BaseDataTableRequest<LocationVM>, IRequest<BaseDataTableResponse<LocationVM>>
    {
    }
}
