using OEEMicroservice.DTOs;
using OEEMicroservice.Models.OEE;
using System.Collections.Generic;

namespace OEEMicroservice.Utils.Calculator
{
    public interface ICalculatorContext
    {
        IEnumerable<DataSet> ExecuteCalculation(Station station, ReportingPeriod reportingPeriod);
        void SetCalculator(CalculationType type);
    }
}