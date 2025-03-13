using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;
using System;

namespace RfidBarcode.Application.Common.Mappings
{
    public class SettingMappingProfile : Profile
    {
        public SettingMappingProfile()
        {
            CreateMap<Domain.Entities.Location, LocationVM>()
                .ReverseMap();

            CreateMap<Gate, GateVM>()
                .ReverseMap();

            CreateMap<GateMapVM, Gate>();

            CreateMap<GateMapVM, GateMap>();

            CreateMap<GateMap, GateMapVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GateId, opt => opt.MapFrom(src => src.GateId))
                .ForMember(dest => dest.PrevLocationId, opt => opt.MapFrom(src => src.PrevLocationId))
                .ForMember(dest => dest.NextLocationId, opt => opt.MapFrom(src => src.NextLocationId))
                .ForMember(dest => dest.PrevLocationName, opt => opt.MapFrom(src => src.NextLocation != null ? src.NextLocation.Name : ""))
                .ForMember(dest => dest.PrevLocationName, opt => opt.MapFrom(src => src.PrevLocation != null ? src.PrevLocation.Name : ""))
                .ForMember(dest => dest.Antenna, opt => opt.MapFrom(src => src.Antenna));

        }
    }
}
