using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface IGlobalRepository
    {
        Task SaveDbContextAsync();
    }
}
