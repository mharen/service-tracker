using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using service_tracker_mvc.Models;
using service_tracker_mvc.Data;

namespace service_tracker_mvc.Controllers
{
    public class InvoiceController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Invoice/

        public ViewResult Index(InvoiceIndexViewModel invoiceIndexViewModel)
        {
            PopulateEditViewBagProperties(includeAllOption: true);

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
            return View(invoiceIndexViewModel);
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
            PopulateEditViewBagProperties(false);

            Invoice Invoice = new Invoice() { ServiceDate = DateTime.UtcNow.Date };
            return View(Invoice);
        }

        //
        // POST: /Invoice/Create

        [HttpPost]
        public ActionResult Create(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        //
        // GET: /Invoice/Edit/5

        public ActionResult Edit(int id)
        {
            PopulateEditViewBagProperties(false);

            Invoice invoice = db.Invoices.Include("Items").Single(i => i.InvoiceId == id);

            for (int i = invoice.Items == null ? 0 : invoice.Items.Count; i < 10; ++i)
            {
                invoice.Items.Add(new InvoiceItem() { InvoiceId = invoice.InvoiceId, InvoiceItemId = i });
            }

            return View(invoice);
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
            // remove incomplete Items
            invoice.Items.RemoveAll(item => item.ProductId <= 0);

            if (ModelState.IsValid)
            {
                //foreach (var Item in invoice.Items)
                //{
                //    //db.Entry(Item).State = Item.InvoiceItemId > 0 ? EntityState.Modified : EntityState.Added;
                //}

                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();

                var ExistingItemIds = db.InvoiceItems.Where(i => i.InvoiceId == invoice.InvoiceId).Select(i => i.InvoiceItemId).ToList();

                foreach (var Item in invoice.Items)
                {
                    db.Entry(Item).State = ExistingItemIds.Contains(Item.InvoiceItemId) ? EntityState.Modified : EntityState.Added;
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