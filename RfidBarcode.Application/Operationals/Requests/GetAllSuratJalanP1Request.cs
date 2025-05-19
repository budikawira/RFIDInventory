using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllSuratJalanP1Request : BaseDataTableRequest<SuratJalanP1VM>, IRequest<BaseDataTableResponse<SuratJalanP1VM>>
    {

    }
}
