using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace RfidBarcode.Application.Operationals.ViewModels
{
    public class StockOpnameVM : BaseViewModel
    {
        public long Id { get; set; }

        public long LocationId { get; set; }
        public string LocationName { get; set; } = null!;
        public int Scanned { get; set; }
        public int Misplaced { get; set; }
        public int NotScanned { get; set; }
        public int InvalidTag { get; set; }
    }
}
