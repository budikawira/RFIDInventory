using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetStockOpnameRequest : BaseObjectRequest<StockOpnameVM>, IRequest<BaseObjectResponse<StockOpnameVM>>
    {
        public GetStockOpnameRequest(StockOpnameVM data) : base(data) { }
    }
}
