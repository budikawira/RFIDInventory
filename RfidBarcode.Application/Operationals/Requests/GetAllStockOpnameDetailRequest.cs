using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllStockOpnameDetailRequest : BaseDataTableRequest<StockOpnameDetailVM>, 
        IRequest<BaseDataTableResponse<StockOpnameDetailVM>>
    {
        public long? StockOpnameId { get; set; }
    }
}
