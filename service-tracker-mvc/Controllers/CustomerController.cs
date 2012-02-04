﻿using System;
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
    public class CustomerController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Customers/

        public ViewResult Index()
        {
            return View(db.Customers.OrderBy(c => c.Name).ToList());
        }

        //
        // GET: /Customers/Details/5

        public ViewResult Details(int id)
        {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        //
        // GET: /Customers/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customers/Create

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        //
        // GET: /Customers/Edit/5

        public ActionResult Edit(int id)
        {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        //
        // POST: /Customers/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //
        // GET: /Customers/Delete/5

        public ActionResult Delete(int id)
        {
            if (db.Invoices.Any(i => i.CustomerId == id))
            {
                ViewBag.DeleteError = "You cannot delete this store because it is tied to existing invoices. You must change the existing invoices to use a different store first";
            }

            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        //
        // POST: /Customers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (db.Invoices.Any(i => i.CustomerId == id))
            {
                throw new InvalidOperationException("You cannot delete this store because it is tied to existing invoices. You must change the existing invoices to use a different store first");
            }
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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