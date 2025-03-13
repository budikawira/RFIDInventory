namespace RfidBarcode.Domain.Services
{
    public class TagSummaryData
    {
        public string Rssi { get; set; } = null!;
        public string Ant { get; set; } = null!;
        public long Time { get; set; }

        public int getRssi()
        {
            try
            {
                return int.Parse(Rssi);
            }
            catch (Exception)
            {
                return -1000;
            }
        }
    }
}
