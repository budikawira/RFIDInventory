using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class DeleteSuratJalanRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }

        public DeleteSuratJalanRequest(long id) 
        {
            Id = id;
        }
    }
}
