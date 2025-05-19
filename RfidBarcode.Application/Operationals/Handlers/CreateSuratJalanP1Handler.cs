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
    public class CreateSuratJalanP1Handler : BaseHandler, IRequestHandler<CreateSuratJalanP1Request, BaseObjectResponse<SuratJalanP1VM>>
    {
        private readonly IUserResolverService _user;
        public CreateSuratJalanP1Handler(IApplicationDbContext context, IMapper mapper, IUserResolverService user)
        {
            _context = context;
            _mapper = mapper;
            _user = user;
        }

        public async Task<BaseObjectResponse<SuratJalanP1VM>> Handle(CreateSuratJalanP1Request request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<SuratJalanP1VM>();
            
            try
            {
                var userId = _user.GetUserId();
                var entity = new SuratJalanP1()
                {
                    Kode = request.Kode,
                    Kode1 = request.Kode1,
                    Kode2 = request.Kode2,
                    Kode3 = request.Kode3,
                    Kode4 = request.Kode4,
                    Grade = request.Grade,
                    Type = request.SuratJalanType,
                    UserId = _user.GetUserId(),
                    //No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count)
                };
                await _context.SuratJalanP1s.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                response.Result = BaseResponse.RESULT_OK;
                response.Message = "Data created successfully!";
                response.Data = _mapper.Map<SuratJalanP1VM>(entity);
            }
            catch (Exception ex)
            {
                response.Message = "Exception : " + ex.Message;
            }

            return response;
        }
    }
}
