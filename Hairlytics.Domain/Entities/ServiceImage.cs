using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class ServiceImage
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public required string Image { get; set; }

        // 🔗 Navigation
        public Service? Service { get; set; }
    }
}
