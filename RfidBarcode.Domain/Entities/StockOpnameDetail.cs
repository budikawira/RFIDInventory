using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RfidBarcode.Domain.Common;
using RfidBarcode.Domain.Entities.Identities;

namespace RfidBarcode.Domain.Entities
{
    public class StockOpnameDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long StockOpnameId { get; set; }

        public string TagId { get; set; } = null!;

        public long? ItemId { get; set; }

        public long? TagLocationId { get; set; }

        public string? FinalLocation { get; set; }

        public string? Note { get; set; }

        public virtual StockOpname StockOpname { get; set; } = null!;
        public virtual Item? Item { get; set; }

        public virtual TagLocation? TagLocation { get; set; }

    }
}
