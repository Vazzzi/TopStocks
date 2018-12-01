using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TopStocks.Models;

namespace TopStocks.Controllers
{
    public class StocksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Stocks/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: Stocks/Manage
        [Authorize(Roles = "Admin")]
        public ActionResult Manage()
        {
            return View(db.Stocks.ToList());
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: Stocks/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Symbol,Description,Price,NextReportDate,Photo")] Stock stock)
        {
            db.Configuration.LazyLoadingEnabled = false;


            string Photo;

            if (ModelState.IsValid && Request.Files.Count > 0)
            {
                Photo = UploadStockPhoto(Request.Files[0]);
                stock.Photo = Photo;
                db.Stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View(stock);
        }

        private string UploadStockPhoto(HttpPostedFileBase uploadedFile)
        {
            string relativePath = "Photos/" + this.User.Identity.Name.Replace("@", "--");
            string absolutePath = AppDomain.CurrentDomain.BaseDirectory + relativePath;

            Directory.CreateDirectory(absolutePath);

            string filename =(new Random().Next()).ToString() +
                              Path.GetExtension(uploadedFile.FileName);
            uploadedFile.SaveAs(Path.Combine(absolutePath, filename));

            return "../" + relativePath + '/' + filename;
        }



        // GET: Stocks/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Symbol,Description,Price,NextReportDate,Photo")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                string Photo;

                if (ModelState.IsValid && Request.Files.Count > 0)
                {
                    Photo = UploadStockPhoto(Request.Files[0]);
                    stock.Photo = Photo;
                }
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(stock);
        }

        // GET: Stocks/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock stock = db.Stocks.Find(id);
            db.Stocks.Remove(stock);
            db.SaveChanges();
            return RedirectToAction("Manage");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult AllStocksJSON()
        {
            var stocks = db.Stocks.ToList();
            return Json(stocks, JsonRequestBehavior.AllowGet);
        }

    }
}
