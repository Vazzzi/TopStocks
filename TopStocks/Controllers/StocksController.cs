using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public void RefreshStocksData()
        {
            
            var stocks = db.Stocks.ToList();
            foreach (var stock in stocks)
            {
                var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/quote";
                IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, stock.Symbol);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    //For IP-API
                    client.BaseAddress = new Uri(IEXTrading_API_PATH);
                    HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var quoteInfo = response.Content.ReadAsAsync<QuoteDataResponse>().GetAwaiter().GetResult();
                        if (quoteInfo != null)
                        {
                            stock.Price.CurrentPrice = quoteInfo.latestPrice;
                            stock.Price.DayLowPrice = quoteInfo.low;
                            stock.Price.DayHighPrice = quoteInfo.high;
                            db.Entry(stock).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }

        }

        public class QuoteDataResponse
        {
            public string symbol { get; set; }
            public string companyName { get; set; }
            public string primaryExchange { get; set; }
            public string sector { get; set; }
            public string calculationPrice { get; set; }
            public double open { get; set; }
            public long openTime { get; set; }
            public double close { get; set; }
            public long closeTime { get; set; }
            public float high { get; set; }
            public float low { get; set; }
            public float latestPrice { get; set; }
            public string latestSource { get; set; }
            public string latestTime { get; set; }
            public long latestUpdate { get; set; }
            public int latestVolume { get; set; }
            public string iexRealtimePrice { get; set; }
            public string iexRealtimeSize { get; set; }
            public string iexLastUpdated { get; set; }
            public double delayedPrice { get; set; }
            public long delayedPriceTime { get; set; }
            public double previousClose { get; set; }
            public string change { get; set; }
            public string changePercent { get; set; }
            public string iexMarketPercent { get; set; }
            public string iexVolume { get; set; }
            public int avgTotalVolume { get; set; }
            public string iexBidPrice { get; set; }
            public string iexBidSize { get; set; }
            public string iexAskPrice { get; set; }
            public string iexAskSize { get; set; }
            public long marketCap { get; set; }
            public double peRatio { get; set; }
            public double week52High { get; set; }
            public double week52Low { get; set; }
            public string ytdChange { get; set; }
        }
    }

}
