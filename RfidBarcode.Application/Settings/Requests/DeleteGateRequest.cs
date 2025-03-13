using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class DeleteGateRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }
        public DeleteGateRequest(long id)
        {
            Id = id;
        }
    }
}
