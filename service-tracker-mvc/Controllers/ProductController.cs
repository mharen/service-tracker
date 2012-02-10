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
    public class ProductController : Controller
    {
        private DataContext db = new DataContext();

        public ViewResult Index()
        {
            return View(db.Products.OrderBy(p => p.Manufacturer).ThenBy(p => p.Description).ToList());
        }

        public ViewResult Details(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            if (db.InvoiceItems.Any(i => i.ProductId == id))
            {
                ViewBag.DeleteError = "You cannot delete this product because it is tied to existing invoices. You must change the existing invoices to use a different product first";
            }
            Product product = db.Products.Find(id);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (db.InvoiceItems.Any(i => i.ProductId == id))
            {
                throw new InvalidOperationException("You cannot delete this product because it is tied to existing invoices. You must change the existing invoices to use a different product first");
            }
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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