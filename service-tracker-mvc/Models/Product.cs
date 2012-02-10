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
    }
}