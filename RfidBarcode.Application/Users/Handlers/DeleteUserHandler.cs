using AutoMapper;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Domain.Entities.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Dynamic.Core;

namespace RfidBarcode.Application.Users.Queries
{
    public class DeleteUserHandler : BaseHandler, IRequestHandler<DeleteUserRequest, 
            BaseResponse>
    {
        public DeleteUserHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<BaseResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();

            try
            {
                var entity = await _context.Users.FindAsync(request.Id);
                if (entity != null)
                {
                    _context.Users.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                    response.Message = "Data removed successfully!";
                    response.Result = BaseResponse.RESULT_OK;
                }
                else
                {
                    response.Message = "Data not found!";
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
