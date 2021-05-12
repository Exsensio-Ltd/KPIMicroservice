using KPIMicroservice.Models;
using KPIMicroservice.Models.OEE;
using System;
using System.Collections.Generic;

namespace KPIMicroservice.Utils.Calculator
{
    public class CalculatorContext
    {
        private IOEECalculator _calculator;

        public void SetCalculator(CalculationType type)
        {
            _calculator = type switch
            {
                CalculationType.Simple => new OEESimpleCalculator(),
                CalculationType.Advanced => new OEEAdvancedCalculator(),
                _ => throw new ArgumentException("Invalid calclation type"),
            };
        }

        public IEnumerable<DataSet> ExecuteCalculation(Station station, int reportingPeriod)
        {
            if (_calculator == null)
            {
                throw new Exception("Calculator type is not selected");
            }
            return _calculator.DataSetConverter(station, reportingPeriod);
        }
    }
}
