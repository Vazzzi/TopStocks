using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TopStocks.Models
{
    public class Holding
    {
        public int ID { get; set; }
        public string StockName {get;set; }
        public int Quantity { get; set; }
        public float BuyingPrice { get; set; }
        public float BuyingValue { get; set; }
        public DateTime BuyingDate { get; set; }
        public virtual int StockID { get; set; }

        public virtual ApplicationUser Buyer { get; set; }
        public virtual Stock Stock { get; set; }
    }
}