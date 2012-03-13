using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        
        [MaxLength(50), Required]
        public string Name { get; set; }
        
        [MaxLength(6), Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string VendorNumber { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
    }
}