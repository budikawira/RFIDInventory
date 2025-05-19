using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllStockOpnameRequest : BaseDataTableRequest<StockOpnameVM>, IRequest<BaseDataTableResponse<StockOpnameVM>>
    {

    }
}
