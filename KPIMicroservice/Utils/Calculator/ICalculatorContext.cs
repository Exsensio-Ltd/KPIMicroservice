using KPIMicroservice.Models;
using KPIMicroservice.Models.OEE;
using System.Collections.Generic;

namespace KPIMicroservice.Utils.Calculator
{
    public interface ICalculatorContext
    {
        IEnumerable<DataSet> ExecuteCalculation(Station station, int reportingPeriod);
        void SetCalculator(CalculationType type);
    }
}