using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace service_tracker_mvc.Models
{
    [NotMapped]
    public class InvoiceFilter
    {
        [DisplayName("Service Date Range")]
        public DateTime? StartDate { get; set; }
        [DisplayName("Service Date Range")]
        public DateTime? EndDate { get; set; }
        [DisplayName("Employee")]
        public int ServicerId { get; set; }
        [DisplayName("Customer")]
        public int CustomerId { get; set; }
        [DisplayName("Key Rec")]
        public string KeyRec { get; set; }
        [DisplayName("FRT Bill")]
        public string FrtBill { get; set; }
        [DisplayName("Purchase Order")]
        public string PurchaseOrder { get; set; }
    }
}