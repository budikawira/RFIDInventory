using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class DeleteItemRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }

        public DeleteItemRequest(long id) 
        {
            Id = id;
        }
    }
}
