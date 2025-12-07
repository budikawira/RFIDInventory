using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class DeleteSuratJalanTypeRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }
        public DeleteSuratJalanTypeRequest(long id)
        {
            Id = id;
        }
    }
}
