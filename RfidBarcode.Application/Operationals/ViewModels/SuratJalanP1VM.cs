using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace RfidBarcode.Application.Operationals.ViewModels
{
    public class SuratJalanP1VM : BaseViewModel
    {
        public long Id { get; set; }
        public string Type { get; set; } = null!;
        public string? No { get; set; } = null!;
        public string? Kode { get; set; }
        public string? Kode1 { get; set; }
        public string? Kode2 { get; set; }
        public string? Kode3 { get; set; }
        public string? Kode4 { get; set; }
        public string? Grade { get; set; }
        public long UserId { get; set; }
        public DateTime? FinalizeDate { get; set; }

        public string Status
        {
            get
            {
                if (FinalizeDate != null)
                {
                    return "Final";
                }

                return "Draft";
            }
        }

        public string FinalizeDateString
        {
            get
            {
                if (FinalizeDate != null)
                {
                    return FinalizeDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "-";
            }
        }
    }
}
