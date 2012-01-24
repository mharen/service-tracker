using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace service_tracker_mvc.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Display(Name = "Employee")]
        public int ServicerId { get; set; }
        public virtual Servicer Servicer { get; set; }

        [Column(TypeName = "DATE")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTime ServiceDate { get; set; }

        [Display(Name = "FRT Bill")]
        public string FrtBill { get; set; }

        [Display(Name = "Key Rec")]
        public string KeyRec { get; set; }

        [Display(Name = "PO No.")]
        public string PurchaseOrder { get; set; }

        public virtual List<InvoiceItem> Items { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Decimal Quantity { get { return Items != null ? Items.Sum(i => i.Quantity) : 0; } }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Total { get { return Items != null ? Items.Sum(i => i.Total) : 0; } }
    }
}