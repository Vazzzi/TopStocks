using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ML.Runtime.Api;
using System.Threading.Tasks;

namespace ML
{
    class StockData
    {
        [Column("0")]
        public float CurrentPrice;

        [Column("1")]
        public float DayHigh;

        [Column("2")]
        public float DayLow;

        [Column("3")]
        [ColumnName("Stock Name")]
        public string StockName;
    }
}
