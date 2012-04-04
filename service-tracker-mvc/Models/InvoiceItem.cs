using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace service_tracker_mvc.Models
{
    public class InvoiceItem
    {
        [Key()]
        [Column(Order = 0)]
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

        [Key()]
        [Column(Order = 1)]
        public int InvoiceItemId { get; set; }

        public decimal Quantity { get; set; }

        public int? ServiceId { get; set; }
        public virtual Service Service { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get { return (Service == null || Quantity == 0)? 0 : Service.Cost * Quantity; } }

        [NotMapped]
        public bool IsEmpty
        {
            get
            {
                return Quantity == 0
                    && (ServiceId ?? 0) == 0;
            }
        }
    }
}