namespace RfidBarcode.Crm.Common.ViewModels.Api.Responses
{
    public class LocationTable
    {
        public int Result { get; set; }
        public string Message { get; set; } = null!;
        public int RecordsTotal { get; set; }
        public List<DataLocationTable> Data { get; set; }

        public LocationTable()
        {
            Result = RESULT_FAILED;
            Message = "";
            Data = new List<DataLocationTable>();
        }

        public const int RESULT_SUCCESS = 0;
        public const int RESULT_FAILED = 1;
    }
}
