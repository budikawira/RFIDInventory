using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RfidBarcode.Domain.Entities
{
    public class ImportItemLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Filename { get; set; } = null!;

        [Required]
        public string Metadata { get; set; } = null!;
    }
}
