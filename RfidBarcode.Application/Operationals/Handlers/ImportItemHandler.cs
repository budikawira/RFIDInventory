using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;
using System.Text.Json.Nodes;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class ImportItemHandler : BaseHandler, IRequestHandler<ImportItemRequest, BaseResponse>
    {
        private readonly IUserResolverService _user;
        private readonly IMediator _mediator;
        public ImportItemHandler(IApplicationDbContext context, IMapper mapper, IUserResolverService user, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _user = user;
            _mediator = mediator;
        }

        public async Task<BaseResponse> Handle(ImportItemRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            var file = request.File;
            var indexColumn = request.IndexColumn;
            var errorList = new List<string>();
            try
            {
                using (var workbook = new XLWorkbook(file.OpenReadStream()))
                {
                    var ws = workbook.Worksheet(1);
                    var rowCount = ws.LastRowUsed()?.RowNumber();

                    using (var transaction = _context.Db.BeginTransaction())
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {

                                var json = new JsonObject();
                                for (int col = 0; col < indexColumn.Count; col++)
                                {
                                    json[indexColumn[col]] = ws.Cell(row, col + 1).GetValue<string>() ?? "";
                                }

                                ItemVM? itemVM = JsonConvert.DeserializeObject<ItemVM>(json.ToJsonString());
                                if (itemVM == null)
                                {
                                    errorList.Add("Error line-" + row + " : Data is null");
                                }
                                else
                                {
                                    itemVM.UserId = _user.GetUserId();
                                    var cmd = new CreateItemRequest(itemVM);
                                    var res = await _mediator.Send(cmd);
                                    if (res.Result != BaseResponse.RESULT_OK)
                                    {
                                        errorList.Add("Error line-" + row + " : " + res.Message);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                errorList.Add("Error line-" + row + " : " + ex.Message);
                            }
                        }


                        if (errorList.Count == 0)
                        {
                            await transaction.CommitAsync();
                            response.Result = BaseResponse.RESULT_OK;
                            response.Message = "Berhasil import data!";
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            response.Message = string.Join("\n", errorList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception: " + ex.Message;
            }

            return response;
        }
    }
}
