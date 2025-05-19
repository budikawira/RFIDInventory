using RfidBarcode.Domain.Common;
using RfidBarcode.Domain.Entities.Identities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace RfidBarcode.Domain.Entities
{
    public class SuratJalanP1 : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string? Type { get; set; } = null!;
        public string? No { get; set; } = null!;
        public string? Kode { get; set; }
        public string? Kode1 { get; set; }
        public string? Kode2 { get; set; }
        public string? Kode3 { get; set; }
        public string? Kode4 { get; set; }
        public string? Grade { get; set; }
        public long UserId { get; set; }
        public DateTime? FinalizeDate { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual List<Item> Items { get; set; } = null!;

        public const string TYPE_P1 = "P1";
        public const string TYPE_K4 = "K4"; 
    }
}
