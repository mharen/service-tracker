﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelGenerator.SpreadSheet;
using ExcelGenerator.SpreadSheet.Styles;
using service_tracker_mvc.ActionResults;
using service_tracker_mvc.Data;
using service_tracker_mvc.Filters;
using service_tracker_mvc.Models;

namespace service_tracker_mvc.Controllers
{

    [RequiresAuthorizationAttribute("Manager")]
    public class InvoiceController : Controller
    {
        private Repo repo = new Repo();

        [RequiresAuthorization("Customer")]
        public ActionResult Index(InvoiceIndexViewModel invoiceIndexViewModel, string button)
        {
            invoiceIndexViewModel = QueryInvoices(invoiceIndexViewModel);

            if (button == "Export")
            {
                return Excel(invoiceIndexViewModel);
            }

            PopulateEditViewBagProperties(includeAllOptionsWhenAppropriate: true);

            return View(invoiceIndexViewModel);
        }

        private static string InvoiceFilterSessionKey = "InvoiceFilterSessionKey";
        public static string ResetFiltersRequestKey = "ResetFilters";

        private InvoiceIndexViewModel QueryInvoices(InvoiceIndexViewModel invoiceIndexViewModel)
        {
            if (invoiceIndexViewModel == null)
            {
                invoiceIndexViewModel = new InvoiceIndexViewModel();
            }

            if (!string.IsNullOrEmpty(Request[ResetFiltersRequestKey]))
            {
                invoiceIndexViewModel.InvoiceFilter = GetDefaultInvoiceFilter();
            }

            if (invoiceIndexViewModel.InvoiceFilter == null)
            {
                var SessionInvoiceFilter = Session[InvoiceFilterSessionKey] as InvoiceFilter;
                invoiceIndexViewModel.InvoiceFilter = SessionInvoiceFilter ?? GetDefaultInvoiceFilter();
            }

            var Filter = invoiceIndexViewModel.InvoiceFilter;

            // apply filters from the view
            invoiceIndexViewModel.Invoices =
                repo.Invoices
                    .Include(i => i.Items)
                    .Include(i => i.Items.Select(item => item.Service))
                    .Include(i => i.Servicer)
                    .Include(i => i.Site)
                    .Include(i => i.Site.Organization)
                    .Where(i => i.ServiceDate >= Filter.StartDate)
                    .Where(i => i.ServiceDate <= Filter.EndDate)
                    .Where(i => Filter.SiteId == 0 || i.SiteId == Filter.SiteId)
                    .Where(i => i.ServicerId == Filter.ServicerId || Filter.ServicerId == 0)
                    .Where(i => Filter.KeyRec == null || Filter.KeyRec == "" || i.KeyRec.Contains(Filter.KeyRec))
                    .Where(i => Filter.FrtBill == null || Filter.FrtBill == "" || i.FrtBill.Contains(Filter.FrtBill))
                    .Where(i => Filter.PurchaseOrder == null || Filter.PurchaseOrder == "" || i.PurchaseOrder.Contains(Filter.PurchaseOrder))
                    // apply user-specific filtering
                    .Where(DataContextExtensions.GetInvoiceFilterForCurrentUser())
                    // sort
                    .OrderBy(i => i.ServiceDate)
                    .ThenBy(i => i.InvoiceId)
                    // evaluate
                    .ToList();

            // save the filter for future use
            Session[InvoiceFilterSessionKey] = invoiceIndexViewModel.InvoiceFilter;

            return invoiceIndexViewModel;
        }

        private static InvoiceFilter GetDefaultInvoiceFilter()
        {
            return new InvoiceFilter()
            {
                SiteId = 0,
                StartDate = DateTime.UtcNow.Date.AddDays(-7),
                EndDate = DateTime.UtcNow.Date,
                ServicerId = 0
            };
        }

        [RequiresAuthorization("Customer")]
        public ActionResult Excel(InvoiceIndexViewModel invoiceIndexViewModel)
        {
            //get the invoices
            invoiceIndexViewModel = QueryInvoices(invoiceIndexViewModel);

            var HeaderStyle = new Style("Header");
            HeaderStyle.font.bold = true;
            StylesManager.addStyle(HeaderStyle);

            // don't show a border by default
            StylesManager.getStyle("Default").border.display = false;

            var Title = string.Format("{0:yyyy.MM.dd}-to-{1:yyyy.MM.dd}", invoiceIndexViewModel.InvoiceFilter.StartDate, invoiceIndexViewModel.InvoiceFilter.EndDate);
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet(Title);

            // add header row
            var Columns = new string[] { "Service Date", "Store", "Employee", "Invoice", "Key REC", "PO", "Invoice Total", 
                                         "SKU", "Service", "Quantity", "Unit Price", "Total Line Item Price" };

            Row HeaderRow = new Row();
            foreach (var CellName in Columns)
            {
                HeaderRow.Cells.Add(new Cell(CellName, "Header"));
            }
            worksheet.Rows.Add(HeaderRow);

            // add content rows
            foreach (var Invoice in invoiceIndexViewModel.Invoices)
            {
                foreach (var Item in Invoice.Items)
                {
                    var Row = new Row();
                    Row.Cells.AddRange(GetInvoiceCells(Invoice));
                    Row.Cells.AddRange(GetInvoiceItemCells(Item));
                    worksheet.Rows.Add(Row);
                }
            }

            //Add worksheet to Workbook
            workbook.Worksheets.Add(worksheet);

            //Return the byte array  
            return new ExcelResult(workbook.getBytes(), Title);
        }

        private IEnumerable<Cell> GetInvoiceCells(Invoice invoice)
        {
            yield return new Cell(invoice.ServiceDate.ToShortDateString());
            yield return new Cell(invoice.Site.ToString());
            yield return new Cell(invoice.Servicer.Name);
            yield return new Cell(invoice.FrtBill);
            yield return new Cell(invoice.KeyRec);
            yield return new Cell(invoice.PurchaseOrder);
            yield return new Cell(invoice.Total.ToString("0.00"));
        }

        private IEnumerable<Cell> GetInvoiceItemCells(InvoiceItem item)
        {
            yield return new Cell(item.Service == null ? "" : item.Service.Sku);
            yield return new Cell(item.Service == null ? "" : item.Service.Description);
            yield return new Cell(item.Quantity.ToString("0.00"));
            yield return new Cell(item.Service == null ? "" : item.Service.Cost.ToString("0.00"));
            yield return new Cell(item.Total.ToString("0.00"));
        }

        [RequiresAuthorization("Customer")]
        public ViewResult Details(int id)
        {
            Invoice invoice = QueryInvoice(id);
            return View(invoice);
        }

        private Invoice QueryInvoice(int id)
        {
            Invoice invoice = repo.Invoices.Include(x => x.Items)
                                         .Include(x => x.Site)
                                         .Include(x => x.Servicer)
                                         .Include(x => x.Items.Select(i => i.Service))
                                         .Single(x => x.InvoiceId == id);

            var filter = DataContextExtensions.GetInvoiceFilterForCurrentUser();
            if (!filter(invoice))
            {
                throw new UnauthorizedAccessException("Your current role or configuration does not allow you to see this record");
            }

            return invoice;
        }

        [RequiresAuthorization("Manager")]
        public ActionResult Create()
        {
            // initialize invoice with defaults
            Invoice Invoice = new Invoice()
            {
                InvoiceId = 0,
                Items = new List<InvoiceItem>()
            };

            PadItemsList(Invoice);
            PopulateEditViewBagProperties(false);
            return View(Invoice);
        }

        [HttpPost]
        [RequiresAuthorization("Manager")]
        public ActionResult Create(Invoice invoice)
        {
            return Edit(invoice);
        }

        [RequiresAuthorization("Manager")]
        public ActionResult Edit(int id)
        {
            PopulateEditViewBagProperties(false);
            Invoice invoice = repo.Invoices.Include(i => i.Items).Single(i => i.InvoiceId == id);
            PadItemsList(invoice);
            return View(invoice);
        }

        private static void PadItemsList(Invoice invoice)
        {
            for (int i = invoice.Items == null ? 0 : invoice.Items.Count; i < 10; ++i)
            {
                invoice.Items.Add(new InvoiceItem() { InvoiceId = invoice.InvoiceId, InvoiceItemId = i });
            }
        }

        private void PopulateEditViewBagProperties(bool includeAllOptionsWhenAppropriate)
        {
            var repo = new Repo();

            ViewBag.Services = repo.Services.ToSelectListItems(includeAllOption: false);
            ViewBag.Servicers = repo.Servicers.ToSelectListItems(includeAllOption: includeAllOptionsWhenAppropriate);
            ViewBag.Sites = repo.Sites.ToSelectListItems(includeAllOption: includeAllOptionsWhenAppropriate);
        }

        [HttpPost]
        [RequiresAuthorization("Manager")]
        public ActionResult Edit(Invoice invoice)
        {
            if (invoice.Items == null)
            {
                invoice.Items = new List<InvoiceItem>();
            }
            else
            {
                invoice.Items.RemoveAll(item => item.IsEmpty);
            }

            if (ModelState.IsValid)
            {
                repo.Entry(invoice).State = invoice.InvoiceId > 0 ? EntityState.Modified : EntityState.Added;
                repo.SaveChanges();

                var ExistingItemIds = repo.InvoiceItems.Where(i => i.InvoiceId == invoice.InvoiceId).Select(i => i.InvoiceItemId).ToList();

                foreach (var Item in invoice.Items)
                {
                    repo.Entry(Item).State = ExistingItemIds.Contains(Item.InvoiceItemId) ? EntityState.Modified : EntityState.Added;
                }
                repo.SaveChanges();
                TempData["Message"] = "Invoice Saved";

                if (Request.Form["Save"] == "Save and Add Another")
                {
                    return RedirectToAction("Create", new { from = Request["from"] });
                }
                if (Request.QueryString["from"] == "details")
                {
                    return RedirectToAction("Details", new { id = invoice.InvoiceId });
                }
                return RedirectToAction("Index");
            }
            PopulateEditViewBagProperties(false);
            return View(invoice);
        }

        [RequiresAuthorization("Manager")]
        public ActionResult Delete(int id)
        {
            Invoice invoice = repo.Invoices.Single(i=>i.InvoiceId == id);
            return View(invoice);
        }

        [RequiresAuthorization("Manager")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = repo.Invoices.Single(i => i.InvoiceId == id); 
            repo.Remove(invoice);
            repo.SaveChanges();
            TempData["Message"] = "Invoice Deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}