using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TopStocks.Controllers
{
    public class StocksController : Controller
    {
        // GET: Stocks
        public ActionResult Index()
        {
            ViewBag.name = "Roi";
            return View();
        }
        public ActionResult Create()
        {
            ViewBag.name = "Roi";
            return View();
        }
    }
}