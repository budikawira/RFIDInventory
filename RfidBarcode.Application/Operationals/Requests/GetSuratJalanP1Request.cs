using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetSuratJalanP1Request : BaseObjectRequest<SuratJalanP1VM>, IRequest<BaseObjectResponse<SuratJalanP1VM>>
    {
        public GetSuratJalanP1Request(SuratJalanP1VM data) : base(data) { }
    }
}
