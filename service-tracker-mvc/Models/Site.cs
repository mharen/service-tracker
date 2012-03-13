using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class Site
    {
        public int SiteId { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }
        
        [DataType(DataType.MultilineText)]
        [MaxLength(250)]
        public string Address { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Organization.Code, Name);
        }
    }
}