using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RfidBarcode.Domain.Common;
using RfidBarcode.Domain.Services;

namespace RfidBarcode.Domain.Entities
{
    public class ItemMovement : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ItemId { get; set; }
        public long? PrevLocationId { get; set; }
        public long? LocationId { get; set; }

        public string? PrevLocationName { get; set; }
        public string? LocationName { get; set; }
        public string? Note { get; set; }
        public string? Source { get; set; }

        public long? TagLocationId { get; set; }

        public virtual Location? PrevLocation { get; set; }

        public virtual Item? Item { get; set; } = null!;
        public virtual Location? Location { get; set; } = null!;

        public const string SOURCE_UPDATE = "UPDT";
        public const string SOURCE_STOCK_OPNAME = "SO";
        public const string SOURCE_GATE = "GATE";
    }
}
