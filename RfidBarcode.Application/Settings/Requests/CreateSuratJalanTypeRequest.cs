using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;

namespace RfidBarcode.Application.Settings.Requests
{
    public class CreateSuratJalanTypeRequest : BaseObjectRequest<SuratJalanTypeVM>, IRequest<BaseObjectResponse<SuratJalanTypeVM>>
    {
        public CreateSuratJalanTypeRequest(SuratJalanTypeVM data) : base(data)
        {
        }
    }
}
