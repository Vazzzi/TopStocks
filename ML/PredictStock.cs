using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;

namespace ML
{
    public class PredictStock
    {
        private PredictionModel<StockData, StockPrediction> model;

        public PredictStock()
        {
            // Creating a pipeline and loading the data
            var pipeline = new LearningPipeline();

            // Pipelining the training file
            string dataPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Profit-Train.txt";
            pipeline.Add(new TextLoader(dataPath).CreateFrom<StockData>(separator: ','));

            // Labeling the data
            pipeline.Add(new Dictionarizer("Label"));

            // Putting features into a vector
            pipeline.Add(new ColumnConcatenator("Features", "CurrentPrice", "DayHigh", "DayLow"));

            // Adding learning algorithm
            pipeline.Add(new StochasticDualCoordinateAscentClassifier());

            // Converting the Label back into original text 
            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });

            // Train the model
            this.model = pipeline.Train<StockData, StockPrediction>();
        }

        public bool PredictStockProfitable(float currentPrice, float dayHigh, int dayLow)
        {
            // Make a prediction
            var prediction = this.model.Predict(new StockData()
            {
                CurrentPrice = currentPrice,
                DayHigh = dayHigh,
                DayLow = dayLow
            });

            return (prediction.PredictedLabels == "true");
        }
    }
}
