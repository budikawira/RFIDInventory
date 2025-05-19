using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.Responses;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class CreateMultipleSuratJalanP1Request : IRequest<CreateMultipleSuratJalanP1Response>
    {
        public List<Int64> Ids { get; set; } = null!;
        public CreateMultipleSuratJalanP1Request()
        {
        }
    }
}
