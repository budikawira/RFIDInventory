using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class AddItemsForP1Request : IRequest<BaseResponse>
    {
        public long SuratJalanP1Id { get; set; }
        public List<long> ItemIds { get; set; }
        public AddItemsForP1Request(long suratJalanP1Id, List<long> itemIds) 
        {
            SuratJalanP1Id = suratJalanP1Id;
            ItemIds = itemIds;
        }
    }
}
