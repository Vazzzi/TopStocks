using System;
using System.Collections.Generic;
using System.Linq;

namespace TopStocks.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int StockCurrentPrice { get; set; }
        public int WeekHigh { get; set; }
        public int WeekLow { get; set; }
        public DateTime NextReportDate { get; set; }


        // Entity Framework can't save an array
        ///public List<string> PhotoList { get; set; }

        //public string Photos
        //{
         //   get => (PhotoList == null || PhotoList.Count <= 0 ? "" : string.Join(",", PhotoList));
         //   set => PhotoList = (string.IsNullOrEmpty(value) ? new List<string>() : value.Split(',').ToList());
        //}
    }
}