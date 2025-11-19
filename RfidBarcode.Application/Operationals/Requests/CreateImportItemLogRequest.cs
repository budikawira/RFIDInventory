using MediatR;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class CreateImportItemLogRequest : IRequest<bool>
    {
        public string Filename { get; set; }
        public string Metadata { get; set; }
        public CreateImportItemLogRequest(string filename, string metadata) 
        { 
            Filename = filename;
            Metadata = metadata;
        }
    }
}
