﻿using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using AutoMapper;

namespace InvoicePro.Infra.IoC.MappingProfiles
{
    public class DesignationProfile : Profile
    {
        public DesignationProfile()
        {
            CreateMap<Designation, DesignationVm>()
             .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
             .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CreatedBy))
             .ForMember(x => x.CreateTime, x => x.MapFrom(x => x.CreateTime))
             .ForMember(x => x.LastModifiedBy, x => x.MapFrom(x => x.LastModifiedBy))
             .ForMember(x => x.LastModifiedTime, x => x.MapFrom(x => x.LastModifiedTime))
             .ForMember(x => x.Name, x => x.MapFrom(x => x.Name))
             .ForMember(x => x.Responsibilities, x => x.MapFrom(x => x.Responsibilities));

            CreateMap<DesignationVm, Designation>()
             .ForMember(x => x.Id, x => x.Ignore())
             .ForMember(x => x.CreatedBy, x => x.Ignore())
             .ForMember(x => x.CreateTime, x => x.Ignore())
             .ForMember(x => x.LastModifiedBy, x => x.Ignore())
             .ForMember(x => x.LastModifiedTime, x => x.Ignore())
             .ForMember(x => x.Name, x => x.MapFrom(x => x.Name))
             .ForMember(x => x.Responsibilities, x => x.MapFrom(x => x.Responsibilities));
        }
    }
}