using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;
using System.Buffers;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class ConfirmSuratJalanRequest : IRequest<BaseResponse>
    {
        public long SuratJalanId { get; set; }

        public ConfirmSuratJalanRequest(long suratJalanId) 
        {
            SuratJalanId = suratJalanId;
        }
    }
}
