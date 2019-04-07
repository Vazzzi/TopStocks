using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TopStocks.Models;


namespace TopStocks.Models
{
    public class HoldingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Holdings
        public ActionResult Index()
        {
            var holdings = db.Holdings.Include(h => h.Stock);
            return View(holdings.Where(hld =>
                hld.Buyer == db.Users.FirstOrDefault<ApplicationUser>(user =>
                    user.UserName == this.User.Identity.Name)));
        }

        // GET: Holdings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holding holding = db.Holdings.Find(id);
            if (holding == null)
            {
                return HttpNotFound();
            }
            return View(holding);
        }

        // GET: Holdings/Create
        /*
        public ActionResult Create()
        {
            ViewBag.StockID = new SelectList(db.Stocks, "ID", "Name");
            return View();
        }
        */
        // POST: Holdings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Stock stock, int quantity, float buyingPrice, float totalSum)
        {
            Holding holding = new Holding();
            Stock stockToBuy = db.Stocks.FirstOrDefault(s => s.ID == stock.ID);
            ApplicationUser currentUser = db.Users.Where(user => user.UserName == User.Identity.Name).FirstOrDefault();
            
            if (currentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            holding.Buyer = currentUser;
            holding.BuyingDate = DateTime.Now;
            holding.BuyingPrice = buyingPrice;
            holding.Quantity = quantity;
            holding.BuyingValue = totalSum;
            holding.StockName = stock.Name;
            holding.Stock = stockToBuy;

            db.Holdings.Add(holding);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }


        // Get: Transactions/TopGainingTable
        [Authorize(Roles = "Admin")]
        public ActionResult TopGainingTable()
        {
            Dictionary<ApplicationUser, int> transactions = db.Holdings.GroupBy(h => h.Buyer)
                                                            .Select(g => new { g.Key, Count = g.Count() })
                                                            .OrderByDescending(s => s.Count)
                                                            .ToDictionary(s => s.Key, s => s.Count);
            return View("Statistics", transactions);
        }
        /*
        
        // GET: Holdings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holding holding = db.Holdings.Find(id);
            if (holding == null)
            {
                return HttpNotFound();
            }
            ViewBag.StockID = new SelectList(db.Stocks, "ID", "Name", holding.StockID);
            return View(holding);
        }

        // POST: Holdings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,CurrentPrice,Quantity,BuyingPrice,BuyingValue,BuyingDate,StockID")] Holding holding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(holding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StockID = new SelectList(db.Stocks, "ID", "Name", holding.StockID);
            return View(holding);
        }
        */
        // GET: Holdings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holding holding = db.Holdings.Find(id);
            if (holding == null)
            {
                return HttpNotFound();
            }
            return View(holding);
        }

        // POST: Holdings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Holding holding = db.Holdings.Find(id);
            db.Holdings.Remove(holding);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
