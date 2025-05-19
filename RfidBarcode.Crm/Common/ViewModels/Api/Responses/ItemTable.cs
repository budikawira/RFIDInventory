using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Crm.Common.ViewModels.Api.Responses
{
    public class ItemTable
    {
        public int Result { get; set; }
        public string Message { get; set; } = null!;
        public int RecordsTotal { get; set; }
        public List<ItemVM> Data { get; set; }

        public ItemTable()
        {
            Result = RESULT_FAILED;
            Message = "";
            Data = new List<ItemVM>();
        }

        public const int RESULT_SUCCESS = 0;
        public const int RESULT_FAILED = 1;
    }
}
