using AutoMapper;
using Hairlytics.Application.DTOs.ServiceDTOs;
using Hairlytics.Domain.Entities;

namespace Hairlytics.Application.Mapping.Configurations
{
    /// <summary>
    /// Maps <see cref="Hairlytics.Application.DTOs.ServiceDTOs"/> ↔ <see cref="Service"/> entity.
    /// Mirrors folder: DTOs/ServiceDTOs/
    /// </summary>
    public static class ServiceMapConfiguration
    {
        public static void Register(Profile profile)
        {
            profile.CreateMap<ServiceCreateDto, Service>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.VendorProfile, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            profile.CreateMap<ServiceUpdateDto, Service>()
                .ForMember(dest => dest.VendorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.VendorProfile, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            profile.CreateMap<Service, ServiceResponseDto>()
                .ForMember(dest => dest.CategoryResponseDto,
                    opt => opt.MapFrom(src => src.Category));
        }
    }
}
