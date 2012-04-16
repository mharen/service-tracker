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
    public class OrganizationController : Controller
    {
        private Repo repo = new Repo();

        public ViewResult Index()
        {
            return View(repo.Organizations.ToList());
        }

        public ViewResult Details(int id)
        {
            Organization organization = repo.Organizations.Single(o => o.OrganizationId == id);
            return View(organization);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Organization organization)
        {
            if (ModelState.IsValid)
            {
                repo.Add(organization);
                repo.SaveChanges();
                TempData["Message"] = "Organization Saved";
                return RedirectToAction("Index");
            }

            return View(organization);
        }

        public ActionResult Edit(int id)
        {
            Organization organization = repo.Organizations.Single(o => o.OrganizationId == id);
            return View(organization);
        }

        [HttpPost]
        public ActionResult Edit(Organization organization)
        {
            if (ModelState.IsValid)
            {
                repo.Entry(organization).State = EntityState.Modified;
                repo.SaveChanges();
                TempData["Message"] = "Organization Saved";
                return RedirectToAction("Index");
            }
            return View(organization);
        }

        public ActionResult Delete(int id)
        {
            if (repo.Sites.Any(s => s.OrganizationId == id))
            {
                ViewBag.DeleteError = "You cannot delete this organization because it is tied to existing sites. You must change the existing sites to use a different organization first or remove them";
            }
            Organization organization = repo.Organizations.Single(o => o.OrganizationId == id);
            return View(organization);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (repo.Sites.Any(s => s.OrganizationId == id))
            {
                throw new InvalidOperationException("You cannot delete this organization because it is tied to existing sites. You must change the existing sites to use a different organization first or remove them");
            }
            Organization organization = repo.Organizations.Single(o => o.OrganizationId == id);
            repo.Remove(organization);
            TempData["Message"] = "Organization Deleted";
            repo.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}