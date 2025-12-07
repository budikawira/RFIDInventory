using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;
using System.Buffers;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class FinalizeP1Request : IRequest<BaseResponse>
    {
        public long SuratJalanId { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public int Sequence { get; set; }

        public FinalizeP1Request(long suratJalanId, string type, string code, int sequence) 
        {
            SuratJalanId = suratJalanId;
            Type = type;
            Code = code;
            Sequence = sequence;
        }
    }
}
