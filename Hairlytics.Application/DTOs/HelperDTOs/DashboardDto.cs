using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.HelperDTOs
{
    public class DashboardDto
    {
        public int TotalTodayBookings { get; set; }
        public int TotalServices { get; set; }
        public int TotalCategories { get; set; }
        public int TotalVendors { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalSubAdmins { get; set; }
    }
}
