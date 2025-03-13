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
    public class CreateItemsHandler : BaseHandler, IRequestHandler<CreateItemsRequest, BaseResponse>
    {
        public CreateItemsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(CreateItemsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<ItemVM>();

            try
            {
                if (request.Data != null)
                {
                    var entities = new List<Item>();
                    foreach (var item in request.Data)
                    {
                        var entity = _mapper.Map<Item>(item);
                        entity.K3l = Helper.GetK3L(item.Kode3 ?? "");
                        entity.Qr = Helper.GetQr(entity);

                        await _context.Items.AddAsync(entity);
                        entities.Add(entity);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    foreach (var e in entities)
                    {
                        e.Epc = Helper.GetEpc(e.Id);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    response.Result = BaseResponse.RESULT_OK;
                    response.Message = "Berhasil menambah " + request.Data.Count + " data!";
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
