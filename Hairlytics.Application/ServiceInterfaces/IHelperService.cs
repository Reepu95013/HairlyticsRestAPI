using Hairlytics.Application.DTOs.HelperDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IHelperService
    {
        Task<DashboardDto> GetDashboardDataCounts();
    }
}
