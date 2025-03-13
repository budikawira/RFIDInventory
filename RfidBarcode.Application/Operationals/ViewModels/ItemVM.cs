using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace RfidBarcode.Application.Operationals.ViewModels
{
    public class ItemVM : BaseViewModel
    {
        public long Id { get; set; }
        public string Merk { get; set; } = null!;
        public string Kp { get; set; } = null!;
        //public string? Ib { get; set; }
        public string? Kode { get; set; }
        public string? Kode1 { get; set; }
        public string? Kode2 { get; set; }
        public string? Kode3 { get; set; }
        public string? Kode4 { get; set; }
        public string? Oz { get; set; }
        public string? Grade { get; set; }
        public decimal? Point { get; set; }
        public decimal? Yard { get; set; }
        public decimal? Kg { get; set; }
        public string? Lebar { get; set; }
        public string? K { get; set; }
        public string? SusutLusi { get; set; }
        public string? SerialNumber { get; set; }
        public string? K3l { get; set; }
        public string? Inisial { get; set; }
        public long UserId { get; set; }
        //public int? Koreksi { get; set; }
        public long? SuratJalanId { get; set; }
        public long? QcFinishUserId { get; set; }
        public DateTime? QcFinish { get; set; }
        public DateTime? TanggalBuatBarcode { get; set; }
        //public string? PointGrade { get; set; } = "";
        public long? SuratJalanP1Id { get; set; }
        public long? ScanP1UserId { get; set; }
        public DateTime? ScanP1 { get; set; }
        //public string? T01 { get; set; }

        public string Lot { get { return ""; } }

        public long? TrackingItemId { get; set; }

        public int PrintCount { get; set; }

        public string TagId { get { return Epc; } }
        public string? Epc { get; set; }
        public string? Qr { get; set; }
        public long? LocationId { get; set; }
        public string? LocationName { get; set; }

        public string TanggalBuatBarcodeString { get { 
                return TanggalBuatBarcode != null ? TanggalBuatBarcode.Value.ToString("yyyy-MM-dd") : ""; 
            } 
        }

        public string PrintStatus
        {
            get
            {
                if (PrintCount > 0) return PRINT_STATUS_DONE;
                return PRINT_STATUS_PENDING;
            }
        }

        public const string PRINT_STATUS_DONE = "Sudah Print";
        public const string PRINT_STATUS_PENDING = "Belum Print";
    }
}
