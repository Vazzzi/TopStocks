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

        // GET: Stocks
        [Authorize]
        public ActionResult Index()
        {
            return View();
                
        }

        // Get: Stocks/Manage
        [Authorize(Roles = "Admin")]
        public ActionResult Manage()
        {
            Dictionary<ApplicationUser, int> transactions = db.Transactions.GroupBy(t => t.Saler)
                                                            .Select(g => new { g.Key, Count = g.Count() })
                                                            .OrderByDescending(s => s.Count)
                                                            .ToDictionary(s => s.Key, s => s.Count);
            return View("Manage", transactions);
        }


        // POST: Apartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include =
                "ID,Owner,Location,Description,PropertyValue,PhotoList,Photos,Balcony,Size,FloorNumber,NumberOfRooms")]
            Apartment apartment)
        {
            db.Configuration.LazyLoadingEnabled = false;


            List<string> PhotoList = new List<string>();

            if (ModelState.IsValid && Request.Files.Count > 0)
            {
                for (int uploadID = 0; uploadID < Request.Files.Count; uploadID++)
                {
                    var uploadedFile = Request.Files[uploadID];

                    if (uploadedFile.HasFile())
                    {
                        PhotoList.Add(UploadStockPhoto(uploadedFile));
                    }
                }

                apartment.PhotoList = PhotoList;
                apartment.Owner = db.Users.FirstOrDefault(s => s.UserName == this.User.Identity.Name);

                db.Stocks.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View(apartment);
        }

        private string UploadStockPhoto(HttpPostedFileBase uploadedFile)
        {
            string relativePath = "UserPhotos/" + this.User.Identity.Name.Replace("@", "------");
            string absolutePath = AppDomain.CurrentDomain.BaseDirectory + relativePath;

            Directory.CreateDirectory(absolutePath);

            string filename = DateTime.Now.ToFileTime() + (new Random().Next()).ToString() +
                              Path.GetExtension(uploadedFile.FileName);
            uploadedFile.SaveAs(Path.Combine(absolutePath, filename));

            return relativePath + '/' + filename;
        }


        

        // GET: Apartments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Apartment apartment = db.Apartments.FirstOrDefault(ap => (ap.ID == id) &&
                                                                      (ap.Owner.UserName == this.User.Identity.Name));

            if (apartment == null)
            {
                return HttpNotFound();
            }

            return View(apartment);
        }

        // POST: Stocks/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock apartment = db.Apartments.FirstOrDefault(ap => (ap.ID == id) &&
                                                                      (ap.Owner.UserName == this.User.Identity.Name));

            if (apartment != null)
            {
                apartment.PhotoList.ForEach(photo => System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photo)));

                db.Apartments.Remove(apartment);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

        public JsonResult AllApartmentsJSON()
        {
            var apartments = db.Apartments.ToList();
            return Json(apartments, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApartmentCountGroupJSON()
        {
            var apartments = db.Apartments.GroupBy(ap => ap.Location.City)
                                          .Select(ap => new { City = ap.Key, Count = ap.Count() });

            return Json(apartments, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SizeBalconyMinOrMaxPriceJSON(int Size, bool Balcony, int MinimumPrice)
        {
            return Json(db.Apartments.Include(t => t.Owner).Where(p => p.Balcony == Balcony && p.Size == Size &&
                                                                       p.PropertyValue >= MinimumPrice).ToList(),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult SizeBalconyPriceRangeJSON(int Size, bool Balcony, int MinimumPrice, int MaximumPrice)
        {
            var QuerySet = db.Apartments.Include(t => t.Owner).Where(p => p.Balcony == Balcony && p.Size == Size &&
                                                                          p.PropertyValue >= MinimumPrice &&
                                                                          p.PropertyValue <= MaximumPrice).ToList();
            return Json(QuerySet, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AmountPerCity(string CityName)
        {
            var QuerySet = db.Apartments.Where(t => t.Location.City == CityName).GroupBy(p => p.Location.City)
                .Select(g => new { count = g.Count() }).ToList();

            return Json(QuerySet, JsonRequestBehavior.AllowGet);
        }

    }
}