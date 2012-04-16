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
    public class SiteController : Controller
    {
        private Repo repo = new Repo();

        public ViewResult Index()
        {
            return View(repo.Sites);
        }

        public ViewResult Details(int id)
        {
            Site site = repo.Sites.Single(s => s.SiteId == id);
            return View(site);
        }

        public ActionResult Create()
        {
            ViewBag.Organizations = repo.Organizations.ToSelectListItems(includeAllOption: false);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Site site)
        {
            if (ModelState.IsValid)
            {
                repo.Add(site);
                repo.SaveChanges();
                TempData["Message"] = "Site Saved";
                return RedirectToAction("Index");
            }

            ViewBag.Organizations = repo.Organizations.ToSelectListItems(includeAllOption: false);
            return View(site);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Organizations = repo.Organizations.ToSelectListItems(includeAllOption: false);
            Site site = repo.Sites.Single(s => s.SiteId == id);
            return View(site);
        }

        [HttpPost]
        public ActionResult Edit(Site site)
        {
            if (ModelState.IsValid)
            {
                repo.Entry(site).State = EntityState.Modified;
                repo.SaveChanges();
                TempData["Message"] = "Site Saved";
                return RedirectToAction("Index");
            }
            ViewBag.Organizations = repo.Organizations.ToSelectListItems(includeAllOption: false);
            return View(site);
        }

        public ActionResult Delete(int id)
        {
            if (repo.Invoices.Any(i => i.SiteId == id))
            {
                ViewBag.DeleteError = "You cannot delete this store because it is tied to existing invoices. You must change the existing invoices to use a different store first";
            }

            Site site = repo.Sites.Single(s => s.SiteId == id);
            return View(site);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (repo.Invoices.Any(i => i.SiteId == id))
            {
                throw new InvalidOperationException("You cannot delete this store because it is tied to existing invoices. You must change the existing invoices to use a different store first");
            }
            Site site = repo.Sites.Single(s => s.SiteId == id);
            repo.Remove(site);
            repo.SaveChanges();
            TempData["Message"] = "Site Deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}