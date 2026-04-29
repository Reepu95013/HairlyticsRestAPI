using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class VendorProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string ShopName { get; set; }
        public required string ShopTelephone { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string City { get; set;}
        public required string Region { get; set; }
        public required string Country { get; set;}
        public required string PostalCode { get; set;}
        public required string TaxNumber { get; set;}        
        public bool Status { get; set; }        
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // 🔗 Navigation
        public ICollection<VendorDocument> Documents { get; set; } = new List<VendorDocument>();      
        public User? User { get; set; }
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<VendorGallery> Gallery { get; set; } = new List<VendorGallery>();
        public ICollection<VendorStaff> VendorStaff { get; set; } = new List<VendorStaff>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();


    }
}
