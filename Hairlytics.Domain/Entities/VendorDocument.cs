using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class VendorDocument
    {
        public int Id { get; set; }        
        public int VendorProfileId { get; set; }        
        public required string DocumentType { get; set; }
        public required string FilePath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public VendorProfile? VendorProfile { get; set; }

    }

}
