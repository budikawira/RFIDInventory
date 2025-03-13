using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class CreateItemPrintLogsRequest : BaseObjectRequest<List<long>>, IRequest<BaseResponse>
    {
        public CreateItemPrintLogsRequest(List<long> data) : base(data) { }
    }
}
