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
    public class AddItemsForSuratJalanHandler : BaseHandler, IRequestHandler<AddItemsForSuratJalanRequest, BaseResponse>
    {
        public AddItemsForSuratJalanHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(AddItemsForSuratJalanRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var suratJalan = await _context.SuratJalans.FindAsync(new object[] { request.SuratJalanId }, cancellationToken);
                if (suratJalan == null)
                {
                    response.Message = "Surat Jalan tidak ditemukan";
                    return response;
                }
                var isInbound = suratJalan.SuratJalanType == SuratJalanType.TYPE_INBOUND;

                var items = await _context.Items.Where(x => request.ItemIds.Contains(x.Id)).ToListAsync();
                if (items.Count == request.ItemIds.Count)
                {
                    if (isInbound)
                    {
                        foreach (var item in items)
                        {
                            item.InSuratJalanId = request.SuratJalanId;
                        }
                    }
                    else
                    {
                        foreach (var item in items)
                        {
                            item.OutSuratJalanId = request.SuratJalanId;
                        }
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
