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
        private DataContext db = new DataContext();

        public ViewResult Index()
        {
            return View(db.Sites.Include(s => s.Organization).OrderBy(c => c.Name).ToList());
        }

        public ViewResult Details(int id)
        {
            Site site = db.Sites.Find(id);
            return View(site);
        }

        public ActionResult Create()
        {
            ViewBag.Organizations = db.Organizations.ToSelectListItems(includeAllOption: false);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Site site)
        {
            if (ModelState.IsValid)
            {
                db.Sites.Add(site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(site);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Organizations = db.Organizations.ToSelectListItems(includeAllOption: false);
            Site site = db.Sites.Find(id);
            return View(site);
        }

        [HttpPost]
        public ActionResult Edit(Site site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }

        public ActionResult Delete(int id)
        {
            if (db.Invoices.Any(i => i.SiteId == id))
            {
                ViewBag.DeleteError = "You cannot delete this store because it is tied to existing invoices. You must change the existing invoices to use a different store first";
            }

            Site site = db.Sites.Find(id);
            return View(site);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Site site = db.Sites.Find(id);
            if (site.Invoices.Any())
            {
                throw new InvalidOperationException("You cannot delete this store because it is tied to existing invoices. You must change the existing invoices to use a different store first");
            }
            db.Sites.Remove(site);
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