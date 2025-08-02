using RfidBarcode.Application.Common.BaseObjects;

namespace RfidBarcode.Application.Reports.ViewModels
{
    public class DailyReportVM : BaseViewModel
    {
        public long Id { get; set; }
        public byte[] Content { get; set; } = null!;
        public DateTime CurrentDate { get; set; } //currentDate for stock status
        public DateTime PreviousDate { get; set; } //previousDate for stock status initial movement

        public string CurrentDateString 
        {
            get
            {
                return CurrentDate.ToString("yyyy-MM-dd");
            }
        }

        public string GetFileName()
        {
            return CurrentDate.ToString("yyyyMMdd") + "-" + PreviousDate.ToString("yyyyMMdd");
        }
    }
}
