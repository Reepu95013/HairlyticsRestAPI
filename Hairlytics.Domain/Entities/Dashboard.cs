using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class Dashboard
    {
        public int TotalTodayBookings { get; set; }
        public int TotalServices { get; set; }
        public int TotalCategories { get; set; }
        public int TotalVendors { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalSubAdmins { get; set; }

    }
}
