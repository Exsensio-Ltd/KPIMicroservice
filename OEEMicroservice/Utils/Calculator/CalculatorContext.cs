using OEEMicroservice.DTOs;
using OEEMicroservice.Models.OEE;
using System;
using System.Collections.Generic;

namespace OEEMicroservice.Utils.Calculator
{
    public class CalculatorContext : ICalculatorContext
    {
        private IOeeCalculator _calculator;

        public void SetCalculator(CalculationType type)
        {
            _calculator = type switch
            {
                CalculationType.Simple => new OeeSimpleCalculator(),
                CalculationType.Advanced => new OeeAdvancedCalculator(),
                _ => throw new ArgumentException("Invalid calculation type"),
            };
        }

        public IEnumerable<DataSet> ExecuteCalculation(Station station, ReportingPeriod reportingPeriod)
        {
            if (_calculator == null)
            {
                throw new Exception("Calculator type is not selected");
            }
            return _calculator.DataSetConverter(station, (int)reportingPeriod);
        }
    }
}
