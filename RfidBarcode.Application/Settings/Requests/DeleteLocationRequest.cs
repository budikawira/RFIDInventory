using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class DeleteLocationRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }
        public DeleteLocationRequest(long id)
        {
            Id = id;
        }
    }
}
