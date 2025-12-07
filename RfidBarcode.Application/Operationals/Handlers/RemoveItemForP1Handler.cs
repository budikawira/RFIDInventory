using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class RemoveItemForP1Handler : BaseHandler, IRequestHandler<RemoveItemsForP1Request, BaseResponse>
    {
        public RemoveItemForP1Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(RemoveItemsForP1Request request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var items = await _context.Items.Where(x => request.ItemIds.Contains(x.Id)).ToListAsync();
                if (items.Count == request.ItemIds.Count)
                {
                    foreach (var item in items)
                    {
                        item.OutSuratJalanId = null;
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = items.Count + " data ditambahkan";

                }
                else
                {
                    response.Message = request.ItemIds.Count - items.Count + " data tidak ditemukan";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return response;
        }
    }
}
