using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RfidBarcode.Domain.Common;
using RfidBarcode.Domain.Entities.Identities;

namespace RfidBarcode.Domain.Entities
{
    public class StockOpname : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? LocationId { get; set; }

        public string? FinalLocationName { get; set; } = null!;
        public long? TrolleyId { get; set; }

        public string? FinalTrolleyName { get; set; } = null!;

        public long? UserId { get; set; }

        public ApplicationUser? User { get; set; } = null!;

        public virtual Location Location { get; set; } = null!;

        public virtual List<StockOpnameDetail> StockOpnameDetails { get; set; } = null!;

    }
}
