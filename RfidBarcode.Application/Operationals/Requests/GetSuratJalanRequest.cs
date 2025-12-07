using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetSuratJalanRequest : BaseObjectRequest<SuratJalanVM>, IRequest<BaseObjectResponse<SuratJalanVM>>
    {
        public GetSuratJalanRequest(SuratJalanVM data) : base(data) { }
    }
}
