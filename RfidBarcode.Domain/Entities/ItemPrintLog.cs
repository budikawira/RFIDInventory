using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RfidBarcode.Domain.Entities
{
    public class ItemPrintLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ItemId { get; set; }

        public Item Item { get; set; } = null!;

    }
}
