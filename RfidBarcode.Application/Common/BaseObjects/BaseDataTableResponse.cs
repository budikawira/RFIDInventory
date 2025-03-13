namespace RfidBarcode.Application.Common.BaseObjects
{
    public class BaseDataTableResponse<T> where T : class
    {
        public string? Draw { get; set; } = null!;
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public int Skip { get; set; }
        public int PageSize { get; set; }
        public List<T> Data { get; set; } = null!;
        public T Entity { get; set; } = null!;
        public bool Status { get; set; } = true;
        public string? Note { get; set; }
        public string? Message { get; set; }

        public DateTime? LastUpdateOn { get; set; }
    }
}
