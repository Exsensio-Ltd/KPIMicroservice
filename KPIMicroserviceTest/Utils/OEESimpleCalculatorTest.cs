﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using KPIMicroservice.Models.OEE;
using KPIMicroservice.Utils.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KPIMicroserviceTest.Utils
{
    [TestClass]
    public class OEESimpleCalculatorTest
    {
        private IOEECalculator _calculator;

        [TestInitialize]
        public void Initialize() => _calculator = new OEESimpleCalculator();

        [TestMethod]
        [DataRow("00:00:01", "00:00:55.314", 1, 60, 92)]
        public void CalculateOEE_InputMetrics_ReturnSimpleOEE(string breakDuration, string idealDuration, int period, int goodProducts, int expected)
        {
            var station = new Station
            {
                ProductionBreakDuration = breakDuration,
                ProductionIdealDuration = idealDuration,
            };
            var metric = new OEEMetric
            {
                ProductionShiftDuration = new TimeSpan(period, 0, 0),
                GoodProductCount = goodProducts
            };

            var (_, _, _, oee) = _calculator.Calculate(station, metric);
            Assert.AreEqual(expected, oee);
        }

        [TestMethod]
        [DataRow("00:00:01", "00:00:55.314", 1, 8, 55, 1)]
        [DataRow("00:00:01", "00:00:55.314", 1, 100, 55, 2)]
        public void GetDataSet_InputMetrics_ReturnOEEDataSet(string breakDuration, string idealDuration, int period, int times, int stepTime, int expected)
        {
            var station = new Station
            {
                ProductionBreakDuration = breakDuration,
                ProductionIdealDuration = idealDuration,
                Metrics = new List<OEEMetric>()
            };

            var date = DateTime.Now;
            var metrics = new List<OEEMetric>();
            for (var i = 0; i < times; i++)
            {
                metrics.Add(new OEEMetric()
                {
                    CreatedTime = date,
                    GoodProductCount = 1
                });
                date = date.AddSeconds(stepTime);
            }
            station.Metrics = metrics;

            var dataSet = _calculator.DataSetConverter(station, period);
            Assert.AreEqual(expected, dataSet.Count());
        }
    }
}
