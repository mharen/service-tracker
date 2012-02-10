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
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Cost { get; set; }
    }
}