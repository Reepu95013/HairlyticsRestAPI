using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.VendorDocumentDTOs
{
    public class VendorDocumentResponseDto
    {
        public int Id { get; set; }
        public required string DocumentType { get; set; }
        public required string FilePath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
