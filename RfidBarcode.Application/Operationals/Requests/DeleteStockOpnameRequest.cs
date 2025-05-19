using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class DeleteStockOpnameRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }

        public DeleteStockOpnameRequest(long id) 
        {
            Id = id;
        }
    }
}
