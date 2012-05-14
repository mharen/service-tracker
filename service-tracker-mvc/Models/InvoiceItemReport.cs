using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace service_tracker_mvc.Models
{
    public class InvoiceItemReport
    {
        public int InvoiceId { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string Store { get; set; }
        public string Employee { get; set; }
        public string FrtBill { get; set; }
        public string KeyRec { get; set; }
        public string PO { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string Sku { get; set; }
        public string Service { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalLineItemPrice { get; set; }
    }
}