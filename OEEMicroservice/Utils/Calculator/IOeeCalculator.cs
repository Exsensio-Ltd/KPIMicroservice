using OEEMicroservice.DTOs;
using OEEMicroservice.Models.OEE;
using System.Collections.Generic;

namespace OEEMicroservice.Utils.Calculator
{
    public interface IOeeCalculator
    {
        (int, int, int, int) Calculate(Station station, OeeMetric metric);
        IEnumerable<DataSet> DataSetConverter(Station station, int reportingPeriod);
    }
}
