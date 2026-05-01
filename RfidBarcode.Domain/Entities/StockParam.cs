using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace RfidBarcode.Domain.Entities
{
    public class StockParam : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(100)]
        public string? c1 { get; set; }
        [MaxLength(100)]
        public string? c2 { get; set; }
        [MaxLength(100)]
        public string? c3 { get; set; }
        [MaxLength(100)]
        public string? c4 { get; set; }
        [MaxLength(100)]
        public string? c5 { get; set; }

        public string? p1 { get; set; }
        public string? p2 { get; set; }
        public string? p3 { get; set; }
        public string? p4 { get; set; }
        public string? p5 { get; set; }
        public string? p6 { get; set; }
        public string? p7 { get; set; }
        public string? p8 { get; set; }
    }
}
