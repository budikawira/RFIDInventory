using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace RfidBarcode.Application.Operationals.ViewModels
{
    public class StockOpnameDetailVM : BaseViewModel
    {
        public long Id { get; set; }
        public long StockOpnameId { get; set; }
        public string? Note { get; set; }
        public string TagId { get; set; } = null!;
        public long? ItemId { get; set; }
        public string? Merk { get; set; }
        public string? Kp { get; set; }
        public string? Kode { get; set; }
        public string? SerialNumber { get; set; }

    }
}
