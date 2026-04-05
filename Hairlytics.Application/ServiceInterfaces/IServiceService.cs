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
        Task<ServiceResponse<string>> AddServiceAsync(ServiceCreateDto serviceCreateDto);
    }
}
