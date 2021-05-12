using KPIMicroservice.Models;
using KPIMicroservice.Models.OEE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KPIMicroservice.Utils.Calculator
{
    public class OEEAdvancedCalculator : IOEECalculator
    {
        public (int, int, int, int) Calculate(Station station, OEEMetric data)
        {
            if (station.TotalProductCount == 0)
            {
                station.TotalProductCount = data.GoodProductCount;
            }

            var breakResult = TimeSpan.TryParse(station.ProductionBreakDuration?.Trim(), out var productionBreakDuration);
            var breakTime = new TimeSpan(0, (int)(breakResult ? productionBreakDuration.TotalMinutes : 0), 0);

            var idealResult = TimeSpan.TryParse(station.ProductionIdealDuration?.Trim(), out var productionIdealDuration);
            var idealDuration = idealResult ? productionIdealDuration.TotalSeconds : 0;

            var runTime = data.ProductionShiftDuration.Subtract(breakTime);

            var availability = runTime.TotalMinutes / data.ProductionShiftDuration.TotalSeconds;
            var performance = idealDuration * station.TotalProductCount / runTime.TotalMinutes;
            var quality = data.GoodProductCount / station.TotalProductCount;

            var oee = availability * performance * quality;

            return (
                Convert.ToInt32(availability * 1000),
                Convert.ToInt32(performance),
                Convert.ToInt32(quality * 10),
                Convert.ToInt32(oee * 100)
            );
        }

        public IEnumerable<DataSet> DataSetConverter(Station station, int reportingPeriod)
        {
            if (!station.Metrics.Any())
            {
                return new List<DataSet>();
            }

            var startTime = station.Metrics.First().CreatedTime;
            var endTime = startTime.AddHours(reportingPeriod);

            var groupedItems = new Dictionary<string, OEEMetric>();
            foreach (var item in station.Metrics)
            {
                var createdTime = item.CreatedTime;
                if (createdTime >= endTime)
                {
                    startTime = createdTime;
                    endTime = startTime.AddHours(reportingPeriod);
                }

                var key = $"{startTime:HH:mm dd-MM}";
                var result = groupedItems.TryGetValue(key, out var metric);
                if (result)
                {
                    metric.GoodProductCount += item.GoodProductCount;
                }
                else
                {
                    item.ProductionShiftDuration = new TimeSpan(reportingPeriod, 0, 0);
                    groupedItems.Add(key, item);
                }
            }

            return groupedItems.Select(i =>
            {
                var (availability, performance, quality, oee) = Calculate(station, i.Value);
                return new DataSet
                {
                    Text = i.Key,
                    Value = oee,
                    Availability = availability,
                    Performance = performance,
                    Quality = quality,
                };
            }).ToList();
        }
    }
}
