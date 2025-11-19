using MediatR;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using Microsoft.EntityFrameworkCore;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class GetIsImportItemDuplicateQuery : IRequestHandler<GetIsImportItemDuplicateRequest, bool>
    {
        private readonly IApplicationDbContext _context;
        public GetIsImportItemDuplicateQuery(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(GetIsImportItemDuplicateRequest request, CancellationToken cancellationToken)
        {
            //check the last imported file
            var lastImport = await _context.ImportItemLogs
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync(cancellationToken);
            var res = false;
            if (lastImport != null)
            {
                if (lastImport.Filename == request.Filename && lastImport.Metadata == request.Metadata)
                {
                    res = true;
                }
            }

            return res;
        }
    }
}
