using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RfidBarcode.Domain.Common;
using RfidBarcode.Domain.Services;

namespace RfidBarcode.Domain.Entities
{
    public class TagLocation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Epc { get; set; } = null!;
        public long? ItemId { get; set; }
        public long? LocationId { get; set; }
        public DateTime StartScanned { get; set; }
        public DateTime? EndScanned { get; set; }
        public DateTime LastScanned { get; set; }

        public string? FinalLocation { get; set; }
        public string? Note { get; set; }
        public string? Source { get; set; }

        public long? PrevLocationId { get; set; }
        public virtual Location? PrevLocation { get; set; }
        
        public long? StockOpnameId { get; set; }
        public virtual StockOpname? StockOpname { get; set; }

        public virtual Item? Item { get; set; } = null!;
        public virtual Location? Location { get; set; } = null!;
    }
}
