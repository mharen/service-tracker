using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace service_tracker_mvc.Models
{
    public class Site
    {
        public int SiteId { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "⇧ Required")]
        public string Name { get; set; }
        
        [DataType(DataType.MultilineText)]
        [MaxLength(250)]
        public string Address { get; set; }

        [DisplayName("Organization")]
        [Required(ErrorMessage = "⇧ Required")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Organization.Code, Name);
        }
    }
}