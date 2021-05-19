using KPIMicroservice.DTOs;
using KPIMicroservice.Models.OEE;
using System.Collections.Generic;

namespace KPIMicroservice.Utils.Calculator
{
    public interface IOeeCalculator
    {
        (int, int, int, int) Calculate(Station station, OeeMetric metric);
        IEnumerable<DataSet> DataSetConverter(Station station, int reportingPeriod);
    }
}
