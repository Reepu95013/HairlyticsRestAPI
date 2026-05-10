using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.RealTime.Interfaces
{
    public interface IDashboardNotifier
    {
        Task DashboardUpdatedAsync();
    }
}
