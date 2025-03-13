

namespace RfidBarcode.Application.Common.BaseObjects
{
    public class BaseDataTableRequest<T> where T : class
    {
        public string? Draw { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public string? SortColumn { get; set; }
        public string? SortColumnDir { get; set; }
        public string? SearchValue { get; set; }
        public string? Action { get; set; }
        public T Data { get; set; } = null!;
        public string? MiscDescription { get; set; }


        public void InitFromDataTable(IFormCollection form)
        {
            Draw = form["draw"].FirstOrDefault();
            var start = form["start"].FirstOrDefault();
            var length = form["length"].FirstOrDefault();
            SortColumn = form["columns[" + form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()!;
            SortColumnDir = form["order[0][dir]"].FirstOrDefault()!;
            SearchValue = form["search[value]"].FirstOrDefault();
            PageSize = length != null ? Convert.ToInt32(length) : 0;
            Skip = start != null ? Convert.ToInt32(start) : 0;

        }
    }
}
