using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace service_tracker_mvc.Models
{
    public class InvoiceIndexViewModel
    {
        public List<Invoice> Invoices { get; set; }
        public InvoiceFilter InvoiceFilter { get; set; }
    }
}