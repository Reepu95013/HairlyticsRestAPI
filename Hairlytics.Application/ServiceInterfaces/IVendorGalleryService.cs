using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.VendroGalleryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IVendorGalleryService
    {
        Task<ServiceResponse<string>> AddVendorGalleryAsync (VendorGalleryCreateDto vendroGalleryCreateDto);
    }
}
