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
            CreateMap<StockOpname, StockOpnameVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : ""))
                .ForMember(dest => dest.Scanned, opt => opt.MapFrom(src =>
                    src.StockOpnameDetails != null ? src.StockOpnameDetails.Where(x => x.Note == "Scanned").Count() : 0))
                .ForMember(dest => dest.NotScanned, opt => opt.MapFrom(src =>
                    src.StockOpnameDetails != null ? src.StockOpnameDetails.Where(x => x.Note == "Not Scanned").Count() : 0))
                .ForMember(dest => dest.Misplaced, opt => opt.MapFrom(src =>
                    src.StockOpnameDetails != null ? src.StockOpnameDetails.Where(x => x.Note == "Misplaced").Count() : 0))
                .ForMember(dest => dest.InvalidTag, opt => opt.MapFrom(src =>
                    src.StockOpnameDetails != null ? src.StockOpnameDetails.Where(x => x.Note == "Invalid Tag").Count() : 0));
            CreateMap<SuratJalan, SuratJalanVM>()
                .ReverseMap();
        }
    }
}
