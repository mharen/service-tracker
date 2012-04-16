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
    public class ServiceController : Controller
    {
        private Repo repo = new Repo();

        public ViewResult Index()
        {
            return View(repo.Services.ToList());
        }

        public ViewResult Details(int id)
        {
            Service service = repo.Services.Single(s => s.ServiceId == id);
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
                repo.Add(service);
                repo.SaveChanges();
                TempData["Message"] = "Service Saved";
                return RedirectToAction("Index");
            }

            return View(service);
        }

        public ActionResult Edit(int id)
        {
            Service service = repo.Services.Single(s => s.ServiceId == id);
            return View(service);
        }

        [HttpPost]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                repo.Entry(service).State = EntityState.Modified;
                repo.SaveChanges();
                TempData["Message"] = "Service Saved";
                return RedirectToAction("Index");
            }
            return View(service);
        }

        public ActionResult Delete(int id)
        {
            if (repo.InvoiceItems.Any(i => i.ServiceId == id))
            {
                ViewBag.DeleteError = "You cannot delete this service because it is tied to existing invoices. You must change the existing invoices to use a different service first";
            }
            Service service = repo.Services.Single(s => s.ServiceId == id);
            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (repo.InvoiceItems.Any(i => i.ServiceId == id))
            {
                throw new InvalidOperationException("You cannot delete this service because it is tied to existing invoices. You must change the existing invoices to use a different service first");
            }
            Service service = repo.Services.Single(s => s.ServiceId == id);
            repo.Remove(service);
            repo.SaveChanges();
            TempData["Message"] = "Service Deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}