using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(6)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string VendorNumber { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
    }
}