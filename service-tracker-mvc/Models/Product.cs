using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required,MaxLength(50)]
        public string Manufacturer { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Manufacturer, !(string.IsNullOrEmpty(Manufacturer) || string.IsNullOrEmpty(Description)) ? " - " : "", Description);
        }
    }
}