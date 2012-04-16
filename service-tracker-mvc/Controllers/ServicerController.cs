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
        private Repo repo = new Repo();

        public ViewResult Index()
        {
            return View(repo.Servicers.ToList());
        }

        public ViewResult Details(int id)
        {
            Servicer servicer = repo.Servicers.Single(s => s.ServicerId == id);
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
                repo.Add(servicer);
                repo.SaveChanges();
                TempData["Message"] = "Employee Saved";
                return RedirectToAction("Index");
            }

            return View(servicer);
        }

        public ActionResult Edit(int id)
        {
            Servicer servicer = repo.Servicers.Single(s => s.ServicerId == id);
            return View(servicer);
        }

        [HttpPost]
        public ActionResult Edit(Servicer servicer)
        {
            if (ModelState.IsValid)
            {
                repo.Entry(servicer).State = EntityState.Modified;
                repo.SaveChanges();
                TempData["Message"] = "Employee Saved";
                return RedirectToAction("Index");
            }
            return View(servicer);
        }

        public ActionResult Delete(int id)
        {
            if (repo.Invoices.Any(i => i.ServicerId == id))
            {
                ViewBag.DeleteError = "You cannot delete this employee because it is tied to existing invoices. You must change the existing invoices to use a different employee first";
            }
            Servicer servicer = repo.Servicers.Single(s => s.ServicerId == id);
            return View(servicer);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (repo.Invoices.Any(i => i.ServicerId == id))
            {
                throw new InvalidOperationException("You cannot delete this employee because it is tied to existing invoices. You must change the existing invoices to use a different employee first");
            }
            Servicer servicer = repo.Servicers.Single(s => s.ServicerId == id);
            repo.Remove(servicer);
            repo.SaveChanges();
            TempData["Message"] = "Employee Deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}