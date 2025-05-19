namespace RfidBarcode.Application.Dashboards.ViewModels
{
    public class SoSummaryVM
    {
        public String LastCompleteSoDate { get; set; } = null!;
        public String LastCompleteSoLocation { get; set; } = null!;
        public int TotalUnknown { get; set; }
    }
}
