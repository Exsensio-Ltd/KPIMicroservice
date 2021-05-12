using KPIMicroservice.Models;
using KPIMicroservice.Models.OEE;
using System.Collections.Generic;

namespace KPIMicroservice.Utils.Calculator
{
    public interface IOEECalculator
    {
        (int, int, int, int) Calculate(Station station, OEEMetric metric);
        IEnumerable<DataSet> DataSetConverter(Station station, int reportingPeriod);
    }
}
