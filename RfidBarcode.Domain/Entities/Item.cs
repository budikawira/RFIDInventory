using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RfidBarcode.Domain.Entities
{
    public class Item : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Merk { get; set; } = null!;

        [Required]
        public string Kp { get; set; } = null!;
        //public string? Ib { get; set; }
        public string? Kode { get; set; }
        public string? Kode1 { get; set; }
        public string? Kode2 { get; set; }
        public string? Kode3 { get; set; }
        public string? Kode4 { get; set; }
        public string? Oz { get; set; }
        public string? Grade { get; set; }
        public string? Point { get; set; }
        public decimal? Yard { get; set; }
        public decimal? Kg { get; set; }
        public string? Lebar { get; set; }
        public string? K { get; set; }
        public string? SusutLusi { get; set; }
        public string? SerialNumber { get; set; }
        public string? K3l { get; set; }
        public string? Inisial { get; set; }
        public long UserId { get; set; }
        //public int Koreksi { get; set; }
        //public int Print { get; set; } = 0;
        public int? R { get; set; } // R = Roll
        public string? IdentitasBenang { get; set; }

        public long? QcFinishUserId { get; set; }
        public DateTime? QcFinish { get; set; }

        [Required]
        public DateTime TanggalBuatBarcode { get; set; }

        //public string? PointGrade { get; set; } = "";
        public long? SuratJalanP1Id { get; set; }
        public long? ScanP1UserId { get; set; }
        public DateTime? ScanP1 { get; set; }

        //public string? T01 { get; set; }
        public long? TrackingItemId { get; set; }

        public string? Epc { get; set; }
        public string? Qr { get; set; }
        public long? LocationId { get; set; }

        public SuratJalanP1? SuratJalanP1 { get; set; }

        public TrackingItem? TrackingItem { get; set; }

        public List<ItemPrintLog> ItemPrintLogs { get; set; } = null!;
        public Location? Location { get; set; }
        public List<TagLocation> TagLocations { get; set; } = null!;
        public List<StockOpnameDetail> StockOpnameDetails { get; set; } = null!;

        public List<ItemMovement> ItemMovements { get; set; } = null!;

    }
}
