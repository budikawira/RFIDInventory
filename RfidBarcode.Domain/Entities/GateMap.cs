using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RfidBarcode.Domain.Common;

namespace RfidBarcode.Domain.Entities
{
    public class GateMap : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long GateId { get; set; }

        public string? Antenna { get; set; }

        public long? PrevLocationId { get; set; }

        public long NextLocationId { get; set; }

        public virtual Gate Gate { get; set; } = null!;

        public virtual Location? PrevLocation { get; set; }

        public virtual Location NextLocation { get; set; } = null!;

    }
}
