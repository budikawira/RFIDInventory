using RfidBarcode.Domain.Common;
using RfidBarcode.Domain.Entities.Identities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace RfidBarcode.Domain.Entities
{
    public class SuratJalan : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string? SuratJalanName { get; set; } = null!;
        public string SuratJalanType { get; set; } = null!;
        public string? No { get; set; } = null!;
        public string? Kode { get; set; }
        public string? Kode1 { get; set; }
        public string? Kode2 { get; set; }
        public string? Kode3 { get; set; }
        public string? Kode4 { get; set; }
        public string? Grade { get; set; }
        public long UserId { get; set; }
        public DateTime? FinalizeDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public int Sequence { get; set; }
        public bool IsReturn { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual List<Item> Items { get; set; } = null!;
    }
}
