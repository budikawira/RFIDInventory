using MediatR;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class GetIsImportItemDuplicateRequest : IRequest<bool>
    {
        public string Filename { get; set; }
        public string Metadata { get; set; }

        public GetIsImportItemDuplicateRequest(string filename, string metadata)
        {
            Filename = filename;
            Metadata = metadata;
        }
    }
}
