using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class ImportItemRequest : IRequest<BaseResponse>
    {
        public IFormFile File;
        public List<string> IndexColumn;
        public ImportItemRequest(IFormFile file, List<string> indexColumn) 
        { 
            File = file;
            IndexColumn = indexColumn;
        }
    }
}
