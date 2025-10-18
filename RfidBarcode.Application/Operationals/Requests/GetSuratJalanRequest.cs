using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetSuratJalanRequest : BaseObjectRequest<SuratJalanP1VM>, IRequest<BaseObjectResponse<SuratJalanP1VM>>
    {
        public GetSuratJalanRequest(SuratJalanP1VM data) : base(data) { }
    }
}
