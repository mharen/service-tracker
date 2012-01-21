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
            }
            return View();
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
            Invoice invoice = db.Invoices.Find(id);
            return View(invoice);
        }

        //
        // POST: /Invoice/Edit/5

        [HttpPost]
        public ActionResult Edit(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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