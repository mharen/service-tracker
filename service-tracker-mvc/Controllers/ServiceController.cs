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
    [Authorize(Roles = "Manager")]
    public class ServiceController : Controller
    {
        private DataContext db = new DataContext();

        public ViewResult Index()
        {
            return View(db.Services.OrderBy(s => s.Sku).ToList());
        }

        public ViewResult Details(int id)
        {
            Service service = db.Services.Find(id);
            return View(service);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service);
        }

        public ActionResult Edit(int id)
        {
            Service service = db.Services.Find(id);
            return View(service);
        }

        [HttpPost]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service);
        }

        public ActionResult Delete(int id)
        {
            if (db.InvoiceItems.Any(i => i.ServiceId == id))
            {
                ViewBag.DeleteError = "You cannot delete this service because it is tied to existing invoices. You must change the existing invoices to use a different service first";
            }
            Service service = db.Services.Find(id);
            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (db.InvoiceItems.Any(i => i.ServiceId == id))
            {
                throw new InvalidOperationException("You cannot delete this service because it is tied to existing invoices. You must change the existing invoices to use a different service first");
            }
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
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