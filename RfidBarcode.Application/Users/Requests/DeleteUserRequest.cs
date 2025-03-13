using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Users.ViewModels;
using MediatR;

namespace RfidBarcode.Application.Users.Requests
{
    public class DeleteUserRequest : IRequest<BaseResponse>
    {
        public long Id { get; set; }
        public DeleteUserRequest(long id)
        {
            Id = id;
        }
    }
}
