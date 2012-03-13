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
        private DataContext db = new DataContext();

        //
        // GET: /Organization/

        public ViewResult Index()
        {
            return View(db.Organizations.ToList());
        }

        //
        // GET: /Organization/Details/5

        public ViewResult Details(int id)
        {
            Organization organization = db.Organizations.Find(id);
            return View(organization);
        }

        //
        // GET: /Organization/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Organization/Create

        [HttpPost]
        public ActionResult Create(Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Organizations.Add(organization);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(organization);
        }
        
        //
        // GET: /Organization/Edit/5
 
        public ActionResult Edit(int id)
        {
            Organization organization = db.Organizations.Find(id);
            return View(organization);
        }

        //
        // POST: /Organization/Edit/5

        [HttpPost]
        public ActionResult Edit(Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organization);
        }

        //
        // GET: /Organization/Delete/5
 
        public ActionResult Delete(int id)
        {
            Organization organization = db.Organizations.Find(id);
            if(organization.Sites.Any())
            {
                ViewBag.DeleteError = "You cannot delete this organization because it is tied to existing sites. You must change the existing sites to use a different organization first or remove them";
            }
            return View(organization);
        }

        //
        // POST: /Organization/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Organization organization = db.Organizations.Find(id);
            if (organization.Sites.Any())
            {
                throw new InvalidOperationException("You cannot delete this organization because it is tied to existing sites. You must change the existing sites to use a different organization first or remove them");
            }
            db.Organizations.Remove(organization);
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