using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RfidBarcode.Domain.Entities
{
    public class TrackingItem : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        public string? Merk { get; set; } = null!;
        public string? Kp { get; set; } = null!;
        public string? Ib { get; set; } = null!;
        public string? Kode { get; set; }
        public string? Kode1 { get; set; } = null!;
        public string? Kode2 { get; set; } = null!;
        public string? Kode3 { get; set; } = null!;
        public string? Kode4 { get; set; } = null!;
        public string? Oz { get; set; } = null!;
        public string? Grade { get; set; } = null!;
        public decimal? Point { get; set; } = null!;
        public decimal? Yard { get; set; }
        public decimal? Kg { get; set; }
        public double? Lebar { get; set; }
        public string? SusutLusi { get; set; } = null!;
        public string? SerialNumber { get; set; } = null!;
        public string? Inisial { get; set; } = null!;
        public DateTime? EncodeTime { get; set; } = null!;
        public string TagId { get { return string.Format("0101{0:X20}", Id); } }
        public long? TrolleyId { get; set; }
        public float? MeterWeaving { get; set; }
        public float? MeterGreige { get; set; }
        public float? MeterBBSF { get; set; }
        public string? WeavingMachineNo { get; set; }

        public DateTime ProductionDate { get; set; }
        public string? FormId { get; set; }
        public string? NoBeamIndigo { get; set; }

        public DateTime? StockOutDate { get; set; }


        public DateTime? ImportTime { get; set; }
        public DateTime? StartProcess { get; set; }
        public DateTime? EndProcess { get; set; }

        public List<Item> Items { get; set; } = null!;
    }
}
