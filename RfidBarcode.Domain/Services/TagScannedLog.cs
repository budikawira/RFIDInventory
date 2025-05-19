using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RfidBarcode.Domain.Common;

namespace RfidBarcode.Domain.Services
{
    public class TagScannedLog
    {
        public long Id { get; set; }
        public string Epc { get; set; } = null!;
        public long ItemId { get; set; }
        public long LocationId { get; set; }
        public string? LocationName { get; set; }
        public long Start { get; set; }
        public long? End { get; set; }
        public long LastScanned { get; set; }
    }
}
