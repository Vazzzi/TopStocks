using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    class StockPrediction
    {
        [Microsoft.ML.Runtime.Api.ColumnName("PredictedLabel")]
        public string PredictedLabels = "";
    }
}
