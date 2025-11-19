using MediatR;
using RfidBarcode.Application.Operationals.Requests;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class CreateImportItemLogHandler : IRequestHandler<CreateImportItemLogRequest, bool>
    {
        private readonly IApplicationDbContext _context;
        public CreateImportItemLogHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateImportItemLogRequest request, CancellationToken cancellationToken)
        {
            var entity = new ImportItemLog
            {
                Filename = request.Filename,
                Metadata = request.Metadata,
            };
            _context.ImportItemLogs.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
