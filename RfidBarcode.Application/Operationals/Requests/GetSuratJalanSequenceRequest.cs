using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetSuratJalanSequenceRequest : IRequest<BaseObjectResponse<Int32>>
    {
        public string Type { get; set; }
        public string Code { get; set; }

        public GetSuratJalanSequenceRequest(string type, string code) 
        {
            Type = type;
            Code = code;
        }
    }
}
