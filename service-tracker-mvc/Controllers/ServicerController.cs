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
    public class ServicerController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Servicers/

        public ViewResult Index()
        {
            return View(db.Servicers.ToList());
        }

        //
        // GET: /Servicers/Details/5

        public ViewResult Details(int id)
        {
            Servicer servicer = db.Servicers.Find(id);
            return View(servicer);
        }

        //
        // GET: /Servicers/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Servicers/Create

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
        
        //
        // GET: /Servicers/Edit/5
 
        public ActionResult Edit(int id)
        {
            Servicer servicer = db.Servicers.Find(id);
            return View(servicer);
        }

        //
        // POST: /Servicers/Edit/5

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

        //
        // GET: /Servicers/Delete/5
 
        public ActionResult Delete(int id)
        {
            Servicer servicer = db.Servicers.Find(id);
            return View(servicer);
        }

        //
        // POST: /Servicers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
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