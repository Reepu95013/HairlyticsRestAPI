using Hairlytics.RealTime.Constants;
using Hairlytics.RealTime.Hubs;
using Hairlytics.RealTime.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.RealTime.Services
{
    public class DashboardNotifier : IDashboardNotifier
    {
        private readonly IHubContext<DashboardHub> _hubContext;

        public DashboardNotifier(
            IHubContext<DashboardHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task DashboardUpdatedAsync()
        {
            await _hubContext.Clients.All.SendAsync(
                SignalREvents.DashboardUpdated);
        }
        
    }
}
