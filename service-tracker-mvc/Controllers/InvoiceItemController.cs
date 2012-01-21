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
    public class InvoiceItemController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /InvoiceItem/

        public ViewResult Index()
        {
            return View(db.InvoiceItems.ToList());
        }

        //
        // GET: /InvoiceItem/Details/5

        public ViewResult Details(int id)
        {
            InvoiceItem invoiceitem = db.InvoiceItems.Find(id);
            return View(invoiceitem);
        }

        //
        // GET: /InvoiceItem/Create

        public ActionResult Create(int id)
        {
            using(var DB = new DataContext()){
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
            return View(new InvoiceItem() { InvoiceId = id });
        } 

        //
        // POST: /InvoiceItem/Create

        [HttpPost]
        public ActionResult Create(InvoiceItem invoiceitem)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceItems.Add(invoiceitem);
                db.SaveChanges();
                return RedirectToAction("Details", "Invoice", new { id = invoiceitem.InvoiceId });  
            }

            return View(invoiceitem);
        }
        
        //
        // GET: /InvoiceItem/Edit/5
 
        public ActionResult Edit(int id)
        {
            InvoiceItem invoiceitem = db.InvoiceItems.Find(id);
            return View(invoiceitem);
        }

        //
        // POST: /InvoiceItem/Edit/5

        [HttpPost]
        public ActionResult Edit(InvoiceItem invoiceitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Invoice", new { id = invoiceitem.InvoiceId });
            }
            return View(invoiceitem);
        }

        //
        // GET: /InvoiceItem/Delete/5
 
        public ActionResult Delete(int id)
        {
            InvoiceItem invoiceitem = db.InvoiceItems.Find(id);
            return View(invoiceitem);
        }

        //
        // POST: /InvoiceItem/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            InvoiceItem invoiceitem = db.InvoiceItems.Find(id);
            db.InvoiceItems.Remove(invoiceitem);
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