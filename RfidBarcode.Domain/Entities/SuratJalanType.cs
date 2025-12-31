using Microsoft.EntityFrameworkCore;
using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace RfidBarcode.Domain.Entities
{
    [Index(nameof(Type))]
    [Index(nameof(Name), IsUnique = true)]
    public class SuratJalanType : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public const string TYPE_INBOUND = "in";
        public const string TYPE_OUTBOND = "out";
        public const string TYPE_OUTBOND_RETURN = "out return";
    }
}
