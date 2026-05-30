using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.ServiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IServiceService
    {
        Task<ServiceResponse<ServiceResponseDto>> GetServiceAsync(int serviceId);
        Task<ServiceResponse<string>> AddServiceAsync(ServiceCreateDto serviceCreateDto);
        Task<ServiceResponse<string>> UpdateServiceAsync(ServiceUpdateDto serviceUpdateDto);
        Task<ServiceResponse<string>> DeleteServiceAsync(int serviceId);
        Task<ServiceResponse<List<ServiceResponseDto>>> GetServiceListAsync(int vendorProfileId);
        Task<ServiceResponse<List<ServiceResponseDto>>> GetServiceListAsync();
        Task<ServiceResponse<List<ServiceResponseDto>>> GetAllServicesAsync();

    }
}
