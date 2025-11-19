using RfidBarcode.Application.Common.BaseObjects;
using System.ComponentModel.DataAnnotations;

namespace RfidBarcode.Application.Operationals.ViewModels
{
    public class TrackingItemVM : BaseViewModel
    {
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
        public string? Point { get; set; } = null!;
        public decimal? Yard { get; set; }
        public decimal? Kg { get; set; }
        public double? Lebar { get; set; }
        public string? SusutLusi { get; set; } = null!;
        public string? SerialNumber { get; set; } = null!;
        public string? Inisial { get; set; } = null!;
        public DateTime? EncodeTime { get; set; } = null!;
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

        public int ItemCount { get; set; }

        public string Status
        {
            get
            {
                if (EndProcess != null)
                {
                    return "Selesai";
                }
                else if (ItemCount > 0)
                {
                    return "Dalam Proses";
                }

                return "Belum Proses";
            }
        }

        public string ImportTimeString
        {
            get
            {
                return ImportTime != null ? ImportTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }

        public string StockOutDateString
        {
            get
            {
                return StockOutDate != null ? StockOutDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }
    }
}
