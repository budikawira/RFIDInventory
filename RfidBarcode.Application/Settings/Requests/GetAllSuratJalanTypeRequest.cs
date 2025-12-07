using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Settings.Requests
{
    public class GetAllSuratJalanTypeRequest : BaseDataTableRequest<SuratJalanTypeVM>, IRequest<BaseDataTableResponse<SuratJalanTypeVM>>
    {
    }
}
