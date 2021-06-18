using System;
using OEEMicroservice.Models.OEE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OEEMicroservice.Utils
{
    public interface IContextClient
    {
        Task CreateEntityAsync(string product, string station, string breakDuration, string idealDuration, DateTime createTime, int totalProductCount = 1);
        Task<Station> GetEntitiesAsync(string stationId);
        IEnumerable<Product> GetProducts();
        Task UpdateStationMeta(string stationId, StationMeta meta);
        Task CheckConnection();
    }
}