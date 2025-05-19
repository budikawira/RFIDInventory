using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RfidBarcode.Domain.Common;

namespace RfidBarcode.Domain.Entities
{
    public class Location : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public byte Type { get; set; }

        public int SkipStockOpname { get; set; }

        public virtual ICollection<GateMap> GateMapPrev { get; set; } = null!;
        public virtual ICollection<GateMap> GateMapNext { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; } = null!;

        public static byte TYPE_NORMAL = 0;
        public static byte TYPE_START_LOCATION = 1;
        public static byte TYPE_END_LOCATION = 2;

        public static int SKIP_STOCKOPNAME_MODE_NO = 0;
        public static int SKIP_STOCKOPNAME_MODE_YES = 1; //location is not mandatory for Stock Opname

    }
}
