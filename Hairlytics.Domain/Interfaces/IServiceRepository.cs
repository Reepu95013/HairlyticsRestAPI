using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface IServiceRepository
    {
        Task AddService(Service service);
        Task<List<Service>> GetServiceList(int vendorProfileId);
        Task<List<Service>> GetServiceList();
        Task<List<Service>> GetServicesByIdsAsync(List<int> serviceIds);

    }
}
