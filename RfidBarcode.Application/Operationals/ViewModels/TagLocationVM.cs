using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace RfidBarcode.Application.Operationals.ViewModels
{
    public class TagLocationVM : BaseViewModel
    {
        public long Id { get; set; }

        public string Epc { get; set; } = null!;
        public long? ItemId { get; set; }
        public long? LocationId { get; set; }
        public DateTime StartScanned { get; set; }
        public DateTime? EndScanned { get; set; }
        public DateTime LastScanned { get; set; }
    }
}
