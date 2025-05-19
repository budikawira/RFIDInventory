using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class DeleteSuratJalanP1Request : IRequest<BaseResponse>
    {
        public long Id { get; set; }

        public DeleteSuratJalanP1Request(long id) 
        {
            Id = id;
        }
    }
}
