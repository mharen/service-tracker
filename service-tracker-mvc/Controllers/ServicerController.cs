using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using service_tracker_mvc.Models;
using service_tracker_mvc.Data;
using service_tracker_mvc.Filters;

namespace service_tracker_mvc.Controllers
{
    [RequiresAuthorizationAttribute("Manager")]
    public class ServicerController : Controller
    {
        private DataContext db = new DataContext();

        public ViewResult Index()
        {
            return View(db.Servicers.OrderBy(s => s.Name).ToList());
        }

        public ViewResult Details(int id)
        {
            Servicer servicer = db.Servicers.Find(id);
            return View(servicer);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Servicer servicer)
        {
            if (ModelState.IsValid)
            {
                db.Servicers.Add(servicer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(servicer);
        }

        public ActionResult Edit(int id)
        {
            Servicer servicer = db.Servicers.Find(id);
            return View(servicer);
        }

        [HttpPost]
        public ActionResult Edit(Servicer servicer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(servicer);
        }

        public ActionResult Delete(int id)
        {
            if (db.Invoices.Any(i => i.ServicerId == id))
            {
                ViewBag.DeleteError = "You cannot delete this employee because it is tied to existing invoices. You must change the existing invoices to use a different employee first";
            }
            Servicer servicer = db.Servicers.Find(id);
            return View(servicer);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (db.Invoices.Any(i => i.ServicerId == id))
            {
                throw new InvalidOperationException("You cannot delete this employee because it is tied to existing invoices. You must change the existing invoices to use a different employee first");
            }
            Servicer servicer = db.Servicers.Find(id);
            db.Servicers.Remove(servicer);
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