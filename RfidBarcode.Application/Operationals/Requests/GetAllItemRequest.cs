using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;
using System.Diagnostics.Eventing.Reader;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetAllItemRequest : BaseDataTableRequest<ItemVM>, IRequest<BaseDataTableResponse<ItemVM>>
    {
        public List<long>? Ids { get; set; }
        public string? PrintStatus { get; set; }
        public long? LocationId { get; set; }

        public string? TanggalProduksi { get; set; }
        public string? Kode { get; set; }

        public long? ExcludedSuratJalanP1Id { get; set; }

        public long? IsForAddInboundItems { get; set; }

        public int StockStatus { get; set; } = ItemVM.STOCK_STATUS_ALL; //default
    }
}
