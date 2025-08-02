using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Reports.ViewModels;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;
using System;

namespace RfidBarcode.Application.Common.Mappings
{
    public class ReportMappingProfile : Profile
    {
        public ReportMappingProfile()
        {
            CreateMap<DailyReport, DailyReportVM>()
                .ReverseMap();
        }
    }
}
