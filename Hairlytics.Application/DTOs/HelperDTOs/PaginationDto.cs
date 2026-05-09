using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.HelperDTOs
{
    public class PaginationDto
    {
        public required int PageNumber { get; set; } = 1;
        public required int PageSize { get; set; } = 10;
    }
}
