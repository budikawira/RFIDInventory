using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RfidBarcode.Domain.Entities
{
    public class DailyReport : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public byte[] Content { get; set; } = null!;
        public DateTime CurrentDate { get; set; } //currentDate for stock status
        public DateTime PreviousDate { get; set; } //previousDate for stock status initial movement
    }
}
