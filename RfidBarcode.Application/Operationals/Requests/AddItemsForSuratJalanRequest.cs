using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class AddItemsForSuratJalanRequest : IRequest<BaseResponse>
    {
        public long SuratJalanId { get; set; }
        public List<long> ItemIds { get; set; }
        public AddItemsForSuratJalanRequest(long suratJalanId, List<long> itemIds) 
        {
            SuratJalanId = suratJalanId;
            ItemIds = itemIds;
        }
    }
}
