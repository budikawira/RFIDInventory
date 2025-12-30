using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;
using System.Buffers;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class UnfinalizeSuratJalanRequest : IRequest<BaseResponse>
    {
        public long SuratJalanId { get; set; }

        public UnfinalizeSuratJalanRequest(long suratJalanId) 
        {
            SuratJalanId = suratJalanId;
        }
    }
}
