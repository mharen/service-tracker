using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int ServicerId { get; set; }
        public virtual Servicer Servicer{ get; set; }

        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; }
        
        public string FrtBill { get; set; }
        
        public string KeyRec { get; set; }
        
        public string PurchaseOrder { get; set; }
        
        public virtual ICollection<InvoiceItem> Items { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Decimal Quantity { get { return Items.Sum(i => i.Quantity); } }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Total { get { return Items.Sum(i => i.Total); } }
    }
}