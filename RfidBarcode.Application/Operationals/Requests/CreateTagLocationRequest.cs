using MediatR;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Operationals.Requests
{
    public class CreateTagLocationRequest : BaseObjectRequest<TagLocationVM>, IRequest<BaseObjectResponse<TagLocationVM>>
    {
        public CreateTagLocationRequest(TagLocationVM data) : base(data) { }
    }
}
