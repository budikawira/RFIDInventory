using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class CreateSuratJalanRequest : IRequest<BaseObjectResponse<SuratJalanVM>>
    {
        public string? SuratJalanType { get; set; }
        public string? Kode { get; set; }
        public string? Kode1 { get; set; }
        public string? Kode2 { get; set; }
        public string? Kode3 { get; set; }
        public string? Kode4 { get; set; }
        public string? Grade { get; set; }

        public CreateSuratJalanRequest()
        {

        }

    }
}
