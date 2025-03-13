using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace RfidBarcode.Domain.Entities
{
    public class Status : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public static long ID_DISABLED = 1;
        public static long ID_ENABLED = 2;
    }
}
