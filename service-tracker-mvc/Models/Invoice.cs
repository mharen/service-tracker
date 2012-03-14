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

        [Display(Name = "Store")]
        [Required(ErrorMessage = "⇧ Required")]
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }

        [Display(Name = "Employee")]
        [Required(ErrorMessage = "⇧ Required")]
        public int ServicerId { get; set; }
        public virtual Servicer Servicer { get; set; }

        [Column(TypeName = "DATE")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        [DisplayName("Date")]
        [Required(ErrorMessage = "⇧ Required")]
        public DateTime ServiceDate { get; set; }

        [Display(Name = "Invoice")]
        [MaxLength(50)]
        public string FrtBill { get; set; }

        [Display(Name = "Key Rec")]
        [MaxLength(50)]
        public string KeyRec { get; set; }

        [Display(Name = "PO No.")]
        [MaxLength(50)]
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