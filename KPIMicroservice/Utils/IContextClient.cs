using KPIMicroservice.Models.OEE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPIMicroservice.Utils
{
    public interface IContextClient
    {
        void Init();
        Task CreateEntityAsync(string product, string station, string breakDuration, string idealDuration, int totalProductCount = 0);
        Task<Station> GetEntitiesAsync(string stationId);
        IEnumerable<Product> GetProducts();
        Task UpdateStationMeta(string stationId, StationMeta meta);
    }
}