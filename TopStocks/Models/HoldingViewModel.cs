using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TopStocks.Models;

namespace TopStocks.Models
{
    public class HoldingViewModel
    {
        public int ID { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public float BuyingPrice { get; set; }
        public float BuyingValue { get; set; }
        public DateTime BuyingDate { get; set; }
        public float CurrentPrice { get; set; }

    }
}