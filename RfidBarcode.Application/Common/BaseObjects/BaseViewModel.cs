namespace RfidBarcode.Application.Common.BaseObjects
{
    public class BaseViewModel
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string? LastUpdateBy { get; set; }

        public string CreatedDateString
        {
            get {
                return CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string LastUpdateDateString
        {
            get { return LastUpdateDate.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
    }
}
