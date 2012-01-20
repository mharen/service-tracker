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
    public class ServiceController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Service/

        public ViewResult Index()
        {
            return View(db.Services.ToList());
        }

        //
        // GET: /Service/Details/5

        public ViewResult Details(int id)
        {
            Service service = db.Services.Find(id);
            return View(service);
        }

        //
        // GET: /Service/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Service/Create

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
        
        //
        // GET: /Service/Edit/5
 
        public ActionResult Edit(int id)
        {
            Service service = db.Services.Find(id);
            return View(service);
        }

        //
        // POST: /Service/Edit/5

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

        //
        // GET: /Service/Delete/5
 
        public ActionResult Delete(int id)
        {
            Service service = db.Services.Find(id);
            return View(service);
        }

        //
        // POST: /Service/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
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