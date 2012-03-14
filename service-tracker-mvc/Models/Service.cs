using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
       
        [MaxLength(50)]
        public string Sku { get; set; }
        
        [MaxLength(50)]
        [Required(ErrorMessage = "⇧ Required")]
        public string Description { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "⇧ Required")]
        public decimal Cost { get; set; }
        
        public override string ToString()
        {
            return string.Format("{0}{1}{2} ({3:c})", Sku, !(string.IsNullOrEmpty(Sku) || string.IsNullOrEmpty(Description)) ? " - " : "", Description, Cost);
        }
    }
}