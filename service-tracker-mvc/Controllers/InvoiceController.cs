using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using service_tracker_mvc.Models;
using service_tracker_mvc.Data;
using System.Text;
using ExcelGenerator.SpreadSheet;
using ExcelGenerator.SpreadSheet.Styles;

namespace service_tracker_mvc.Controllers
{
    //[Authorize(Roles="Manager")]
    public class InvoiceController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Index(InvoiceIndexViewModel invoiceIndexViewModel, string button)
        {
            invoiceIndexViewModel = QueryInvoices(invoiceIndexViewModel);

            if(button == "Export"){
                return Excel(invoiceIndexViewModel);
            }

            PopulateEditViewBagProperties(includeAllOption: true);

            return View(invoiceIndexViewModel);
        }

        private InvoiceIndexViewModel QueryInvoices(InvoiceIndexViewModel invoiceIndexViewModel)
        {
            if (invoiceIndexViewModel == null)
            {
                invoiceIndexViewModel = new InvoiceIndexViewModel();
            }

            if (invoiceIndexViewModel.InvoiceFilter == null)
            {
                invoiceIndexViewModel.InvoiceFilter = new InvoiceFilter()
                {
                    CustomerId = 0,
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.Date,
                    ServicerId = 0
                };
            }

            var Filter = invoiceIndexViewModel.InvoiceFilter;

            invoiceIndexViewModel.Invoices = db.Invoices
                                                .Where(i => i.ServiceDate >= Filter.StartDate)
                                                .Where(i => i.ServiceDate <= Filter.EndDate)
                                                .Where(i => i.CustomerId == Filter.CustomerId || Filter.CustomerId == 0)
                                                .Where(i => i.CustomerId == Filter.ServicerId || Filter.ServicerId == 0)
                                                .Where(i => i.KeyRec.Contains(Filter.KeyRec) || Filter.KeyRec == null || Filter.KeyRec == "")
                                                .Where(i => i.KeyRec.Contains(Filter.FrtBill) || Filter.FrtBill == null || Filter.FrtBill == "")
                                                .Where(i => i.KeyRec.Contains(Filter.PurchaseOrder) || Filter.PurchaseOrder == null || Filter.PurchaseOrder == "")
                                                .OrderBy(i => i.ServiceDate)
                                                .ToList();
            return invoiceIndexViewModel;
        }

//        public FileContentResult Csv(InvoiceIndexViewModel invoiceIndexViewModel)
//        {
//            string Csv = "";
//// TODO
//            var CsvBytes = ASCIIEncoding.ASCII.GetBytes(Csv);
//            var Filename = string.Format("invoices-{0:yyyy.MM.dd}-to-{1:yyyy.MM.dd}", invoiceIndexViewModel.InvoiceFilter.StartDate, invoiceIndexViewModel.InvoiceFilter.EndDate);
//            return File(CsvBytes, "text/csv", Filename);
//        }

        public ActionResult Excel(InvoiceIndexViewModel invoiceIndexViewModel)
        {
            //get the invoices
            invoiceIndexViewModel = QueryInvoices(invoiceIndexViewModel);

            var HeaderStyle = new Style("Header");
            HeaderStyle.font.bold = true;
            StylesManager.addStyle(HeaderStyle);

            // don't show a border by default
            StylesManager.getStyle("Default").border.display = false;
            
            var Title = string.Format("{0:yyyy.MM.dd}-to-{1:yyyy.MM.dd}", invoiceIndexViewModel.InvoiceFilter.StartDate, invoiceIndexViewModel.InvoiceFilter.EndDate);
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet(Title);
            
            // add header row
            var Columns = new string[] { "Service Date", "Customer/Site", "Servicer", "FRT Bill", "Key REC", "PO", "Invoice Total", 
                                         "Product", "Comment", "Service", "SKU", "Quantity", "Unit Price", "Total Line Item Price" };

            Row HeaderRow = new Row();
            foreach (var CellName in Columns)
            {
                HeaderRow.Cells.Add(new Cell(CellName, "Header"));
            }
            worksheet.Rows.Add(HeaderRow);

            // add content rows
            foreach (var Invoice in invoiceIndexViewModel.Invoices)
            {
                foreach (var Item in Invoice.Items)
                {
                    var Row = new Row();
                    Row.Cells.AddRange(GetInvoiceCells(Invoice));
                    Row.Cells.AddRange(GetInvoiceItemCells(Item));
                    worksheet.Rows.Add(Row);
                }
            }
            
            //Add worksheet to Workbook
            workbook.Worksheets.Add(worksheet);

            //Return the byte array  
            return new ExcelResult(workbook.getBytes(), Title);
        }

        private IEnumerable<Cell> GetInvoiceCells(Invoice invoice)
        {
            yield return new Cell(invoice.ServiceDate.ToShortDateString());
            yield return new Cell(invoice.Customer.Name);
            yield return new Cell(invoice.Servicer.Name);
            yield return new Cell(invoice.FrtBill);
            yield return new Cell(invoice.KeyRec);
            yield return new Cell(invoice.PurchaseOrder);
            yield return new Cell(invoice.Total.ToString("0.00"));
        }

        private IEnumerable<Cell> GetInvoiceItemCells(InvoiceItem item)
        {
            yield return new Cell(item.Product.Description);
            yield return new Cell(item.Comment);
            yield return new Cell(item.Service.Description);
            yield return new Cell(item.Service.Sku);
            yield return new Cell(item.Quantity.ToString("0.00"));
            yield return new Cell(item.Service.Cost.ToString("0.00"));
            yield return new Cell(item.Total.ToString("0.00"));
        }

        public ViewResult Details(int id)
        {
            Invoice invoice = db.Invoices.Include(x => x.Items)
                                         .Include(x => x.Customer)
                                         .Include(x => x.Servicer)
                                         .Single(x => x.InvoiceId == id);
            return View(invoice);
        }

        //
        // GET: /Invoice/Create

        public ActionResult Create()
        {
            // initialize invoice with defaults
            Invoice Invoice = new Invoice()
            {
                ServiceDate = DateTime.UtcNow.Date,
                InvoiceId = 0,
                Items = new List<InvoiceItem>()
            };

            PadItemsList(Invoice);
            PopulateEditViewBagProperties(false);
            return View("Edit", Invoice);
        }

        //
        // POST: /Invoice/Create

        [HttpPost]
        public ActionResult Create(Invoice invoice)
        {
            return Edit(invoice);
        }

        //
        // GET: /Invoice/Edit/5

        public ActionResult Edit(int id)
        {
            PopulateEditViewBagProperties(false);
            Invoice invoice = db.Invoices.Include("Items").Single(i => i.InvoiceId == id);
            PadItemsList(invoice);
            return View(invoice);
        }

        private static void PadItemsList(Invoice invoice)
        {
            for (int i = invoice.Items == null ? 0 : invoice.Items.Count; i < 10; ++i)
            {
                invoice.Items.Add(new InvoiceItem() { InvoiceId = invoice.InvoiceId, InvoiceItemId = i });
            }
        }

        private void PopulateEditViewBagProperties(bool includeAllOption)
        {
            SelectListItem[] AllOptions = includeAllOption
                ? new SelectListItem[] { new SelectListItem { Text = "All", Value = "0" } }
                : new SelectListItem[] { };

            ViewBag.Customers = AllOptions.Union(
                                    db.Customers.ToList()
                                          .Select(p => new SelectListItem()
                                          {
                                              Text = string.Format("{0} - {1}", p.Name, p.VendorNumber),
                                              Value = p.CustomerId.ToString()
                                          })
                                ).ToList();

            ViewBag.Servicers = AllOptions.Union(
                                    db.Servicers.ToList()
                                          .Select(s => new SelectListItem()
                                          {
                                              Text = s.Name,
                                              Value = s.ServicerId.ToString()
                                          })
                                ).ToList();

            ViewBag.Products = AllOptions.Select(p => new
                                          {
                                              Text = p.Text,
                                              Value = p.Value
                                          })
                                         .Union(
                                            db.Products.ToList()
                                              .Select(p => new
                                              {
                                                  Text = string.Format("{0} - {1}", p.Manufacturer, p.Description),
                                                  Value = p.ProductId.ToString()
                                              })
                                ).ToList();

            ViewBag.Services = AllOptions.Select(p => new ExtendedSelectListItem()
                                          {
                                              Text = p.Text,
                                              Value = p.Value,
                                              htmlAttributes = new { data_cost = 0m }
                                          })
                                         .Union(
                                            db.Services.ToList()
                                              .Select(s => new ExtendedSelectListItem()
                                              {
                                                  Text = string.Format("{0} - {1} ({2:c})", s.Sku, s.Description, s.Cost),
                                                  Value = s.ServiceId.ToString(),
                                                  htmlAttributes = new { data_cost = s.Cost }
                                              })
                                ).ToList();
        }

        //
        // POST: /Invoice/Edit/5

        [HttpPost]
        public ActionResult Edit(Invoice invoice)
        {
            if (invoice.Items == null)
            {
                invoice.Items = new List<InvoiceItem>();
            }
            else
            {
                // remove incomplete Items
                invoice.Items.RemoveAll(item => item.ProductId <= 0);
            }

            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = invoice.InvoiceId > 0 ? EntityState.Modified : EntityState.Added;
                db.SaveChanges();

                var ExistingItemIds = db.InvoiceItems.Where(i => i.InvoiceId == invoice.InvoiceId).Select(i => i.InvoiceItemId).ToList();

                foreach (var Item in invoice.Items)
                {
                    db.Entry(Item).State = ExistingItemIds.Contains(Item.InvoiceItemId) ? EntityState.Modified : EntityState.Added;
                    // should i set the item.invoiceid prop, too?
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            PopulateEditViewBagProperties(false);
            return View(invoice);
        }

        //
        // GET: /Invoice/Delete/5

        public ActionResult Delete(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            return View(invoice);
        }

        //
        // POST: /Invoice/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}