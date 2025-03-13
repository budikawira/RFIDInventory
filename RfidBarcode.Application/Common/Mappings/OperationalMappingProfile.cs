using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;
using System;
using Item = RfidBarcode.Domain.Entities.Item;

namespace RfidBarcode.Application.Common.Mappings
{
    public class OperationalMappingProfile : Profile
    {
        public OperationalMappingProfile()
        {
            CreateMap<Item, ItemVM>()
                .ReverseMap();
            CreateMap<TrackingItem, TrackingItemVM>()
                .ReverseMap();
            CreateMap<TagLocation, TagLocationVM>()
                .ReverseMap();
        }
    }
}
