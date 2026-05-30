using Microsoft.AspNetCore.Http;

namespace Hairlytics.Application.DTOs.ServiceDTOs
{
    public class ServiceUpdateDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public required string ServiceName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? Image { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public decimal? OffPrice { get; set; }
        public required string Description { get; set; }
        public bool Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
