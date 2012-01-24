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

        public ViewResult Index()
        {
            return View(db.Invoices.ToList());
        }

        //
        // GET: /Invoice/Details/5

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
            using (var DB = new DataContext())
            {
                ViewBag.Customers = DB.Customers.ToList()
                                              .Select(p => new SelectListItem()
                                              {
                                                  Text = string.Format("{0} - {1}", p.Name, p.VendorNumber),
                                                  Value = p.CustomerId.ToString()
                                              });

                ViewBag.Servicers = DB.Servicers.ToList()
                                              .Select(s => new SelectListItem()
                                              {
                                                  Text = s.Name,
                                                  Value = s.ServicerId.ToString()
                                              });

                ViewBag.Products = DB.Products.ToList()
                                              .Select(p => new SelectListItem()
                                              {
                                                  Text = string.Format("{0} - {1}", p.Manufacturer, p.Description),
                                                  Value = p.ProductId.ToString()
                                              });

                ViewBag.Services = DB.Services.ToList()
                                              .Select(s => new SelectListItem()
                                              {
                                                  Text = string.Format("{0} - {1} ({2:c})", s.Sku, s.Description, s.Cost),
                                                  Value = s.ServiceId.ToString()
                                              });
            }

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
            PopulateEditViewBagProperties();

            Invoice invoice = db.Invoices.Include("Items").Single(i => i.InvoiceId == id);

            for (int i = invoice.Items == null ? 0 : invoice.Items.Count; i < 10; ++i)
            {
                invoice.Items.Add(new InvoiceItem() { InvoiceId = invoice.InvoiceId, InvoiceItemId = i });
            }

            return View(invoice);
        }

        private void PopulateEditViewBagProperties()
        {
            ViewBag.Customers = db.Customers.ToList()
                                          .Select(p => new SelectListItem()
                                          {
                                              Text = string.Format("{0} - {1}", p.Name, p.VendorNumber),
                                              Value = p.CustomerId.ToString()
                                          });

            ViewBag.Servicers = db.Servicers.ToList()
                                          .Select(s => new SelectListItem()
                                          {
                                              Text = s.Name,
                                              Value = s.ServicerId.ToString()
                                          });

            ViewBag.Products = db.Products.ToList()
                                          .Select(p => new
                                          {
                                              Text = string.Format("{0} - {1}", p.Manufacturer, p.Description),
                                              Value = p.ProductId.ToString()
                                          });

            ViewBag.Services = db.Services.ToList()
                                          .Select(s => new ExtendedSelectListItem()
                                          {
                                              Text = string.Format("{0} - {1} ({2:c})", s.Sku, s.Description, s.Cost),
                                              Value = s.ServiceId.ToString(),
                                              htmlAttributes = new { data_cost = s.Cost }
                                          });
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
            PopulateEditViewBagProperties();
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