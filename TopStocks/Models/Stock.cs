using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopStocks.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Price Price { get; set; }
        public DateTime NextReportDate { get; set; }
        public string Photo { get; set; }

    }
}