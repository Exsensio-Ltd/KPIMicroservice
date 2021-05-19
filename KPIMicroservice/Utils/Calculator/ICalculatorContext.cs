using KPIMicroservice.DTOs;
using KPIMicroservice.Models.OEE;
using System.Collections.Generic;

namespace KPIMicroservice.Utils.Calculator
{
    public interface ICalculatorContext
    {
        IEnumerable<DataSet> ExecuteCalculation(Station station, ReportingPeriod reportingPeriod);
        void SetCalculator(CalculationType type);
    }
}