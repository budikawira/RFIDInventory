using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class FinalizeP1Request : IRequest<BaseResponse>
    {
        public long SuratJalanP1Id { get; set; }
        public string Type { get; set; }
        public string No { get; set; }

        public FinalizeP1Request(long suratJalanP1Id, string type, string no) 
        {
            SuratJalanP1Id = suratJalanP1Id;
            Type = type;
            No = no;
        }
    }
}
