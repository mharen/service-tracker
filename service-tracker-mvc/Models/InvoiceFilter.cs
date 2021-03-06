﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using service_tracker_mvc.Classes;

namespace service_tracker_mvc.Models
{
    [NotMapped]
    public class InvoiceFilter
    {
        [Required, DisplayName("Service Date Range")]
        public DateTime? StartDate { get; set; }

        [Required, DisplayName("Service Date Range")]
        public DateTime? EndDate { get; set; }

        [Required, DisplayName("Apply Dates To")]
        public DateFilterType DateFilterType { get; set; }
        
        [DisplayName("Employee")]
        public int ServicerId { get; set; }

        [DisplayName("Organization")]
        public int OrganizationId { get; set; }

        [DisplayName("Store")]
        public int SiteId { get; set; }
        
        [DisplayName("Key Rec")]
        [MaxLength(50)]
        public string KeyRec { get; set; }
        
        [DisplayName("Invoice")]
        [MaxLength(50)]
        public string FrtBill { get; set; }
        
        [DisplayName("PO")]
        [MaxLength(50)]
        public string PurchaseOrder { get; set; }
    }
}