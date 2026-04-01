using AutoMapper;
using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.DTOs.VendorDocumentDTOs;
using Hairlytics.Application.DTOs.VendorProfileDTOs;
using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            // =========================
            // USER MAPPINGS
            // =========================

            // Entity → Response DTO
            CreateMap<User, UserCreateDto>();
            CreateMap<User, UserResponseDto>();

            // Create DTO → Entity
            CreateMap<UserCreateDto, User>();

            // Update DTO → Entity
            CreateMap<UserUpdateDto, User>();


            // =========================
            // VENDOR PROFILE MAPPINGS
            // =========================

            // Entity → Response DTO
            CreateMap<VendorProfile, VendorProfileResponseDto>();

            // Create DTO → Entity
            CreateMap<VendorProfileCreateDto, VendorProfile>();
           
            // Update DTO → Entity
            CreateMap<VendorProfileUpdateDto, VendorProfile>();


            // =========================
            // VENDOR DOCUMENT MAPPINGS 
            // =========================

            CreateMap<VendorDocument, VendorDocumentResponseDto>();           
            CreateMap<VendorDocumentCreateDto, VendorDocument>();


            // create category DTO to Category 

            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryResponseDto>();




        }

    }
}
