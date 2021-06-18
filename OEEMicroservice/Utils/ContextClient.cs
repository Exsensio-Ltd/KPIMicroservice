using OEEMicroservice.Models.OEE;
using OEEMicroservice.Serializers;
using OEEMicroservice.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OEEMicroservice.Utils
{
    public class ContextClient : IContextClient, IDisposable
    {
        #region Fields

        private const string EntityUrl = "v2/entities";
        private static readonly ConcurrentDictionary<string, Product> Products = new();
        private static readonly ConcurrentDictionary<string, Station> Stations = new();
        private readonly HttpClientHandler _httpHandler = new();
        private HttpClient _client;
        private bool _disposed;

        #endregion

        #region Constructors

        public ContextClient()
        {
            var baseUrl = Environment.GetEnvironmentVariable("CONTEXT_URL");
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new Exception("Base URL is not present in environment");
            }

            _client = new HttpClient(_httpHandler, true)
            {
                BaseAddress = new Uri(baseUrl)
            };
            Task.WaitAll(LoadProductsAsync(), LoadStationsAsync());
        }

        #endregion

        #region Methods

        public async Task CreateEntityAsync(string product, string station, string breakDuration, string idealDuration, DateTime createTime, int totalProductCount = 1)
        {
            var productId = await TryAddProductAsync(product);
            var stationId = await TryAddStationAsync(station, productId, breakDuration, idealDuration, totalProductCount);

            await UpdateStationMeta(stationId, new StationMeta
            {
                ProductionBreakDuration = breakDuration,
                ProductionIdealDuration = idealDuration,
                TotalProductCount = totalProductCount
            });

            var entity = new OeeMetric
            {
                Id = Guid.NewGuid().ToString(),
                RefStation = stationId,
                CreatedTime = createTime
            };

            var content = new StringContent(JsonSerializer.Serialize(entity), System.Text.Encoding.UTF8, "application/json");
            await _client.PostAsync(EntityUrl, content);
        }

        public async Task UpdateStationMeta(string stationId, StationMeta meta)
        {
            var result = Stations.FirstOrDefault(s => s.Value.GetFullId() == stationId);
            if (!meta.IsUpdated(result.Value))
                return;

            var json = JsonSerializer.Serialize(meta, new JsonSerializerOptions
            {
                IgnoreNullValues = true
            });
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{EntityUrl}/{stationId}/attrs", content);
            await response.Content.ReadAsStringAsync();
        }

        public IEnumerable<Product> GetProducts()
        {
            return Products.Select(p => new Product
            {
                Id = p.Value.Id,
                Type = EntityType.Product,
                Name = p.Value.Name,
                Stations = Stations
                    .Where(s => s.Value.RefProduct == p.Value.GetFullId())
                    .Select(s => new Station
                    {
                        Id = s.Value.Id,
                        Type = EntityType.Station,
                        Name = s.Value.Name,
                        RefProduct = s.Value.RefProduct
                    })
                    .ToList()
            }).ToList();
        }

        public async Task<Station> GetEntitiesAsync(string stationId)
        {
            var parametersStation = BuildParams(new Dictionary<string, string>
            {
                { "options", "keyValues" },
                { "type", EntityType.Station },
            });

            var responseStation = await _client.GetAsync($"{EntityUrl}/{stationId}?{parametersStation}");
            responseStation.EnsureSuccessStatusCode();
            var station = await responseStation.Content.ReadAsObjectAsync<Station>();

            var parametersMetric = BuildParams(new Dictionary<string, string>
            {
                { "options", "keyValues" },
                { "type", EntityType.OeeMetric },
                { "limit", "1000" },
                { "refStation", stationId }
            });

            var responseMetric = await _client.GetAsync($"{EntityUrl}?{parametersMetric}");
            responseMetric.EnsureSuccessStatusCode();
            var metrics = await responseMetric.Content.ReadAsObjectAsync<IEnumerable<OeeMetric>>();

            station.Metrics = metrics;

            return station;
        }

        public async Task CheckConnection()
        {
            var response = await _client.GetAsync("/version");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsObjectAsync<ContextVersion>();
            Console.WriteLine($"Connected: v{result.Orion.Version}, Uptime: {result.Orion.Uptime}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _client.Dispose();
                }

                _client = null;
                _disposed = true;
            }
        }

        #endregion

        #region Private Methods

        private async Task LoadProductsAsync()
        {
            var parameters = new Dictionary<string, string>
            {
                { "options", "keyValues" },
                { "type", EntityType.Product },
            };

            var url = $"{EntityUrl}?{BuildParams(parameters)}";
            var response = await _client.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
                var entities = await response.Content.ReadAsObjectAsync<List<Product>>();
                foreach (var entity in entities)
                {
                    Products.TryAdd(entity.Name, entity);
                }
            }
            catch (HttpRequestException)
            { }
        }

        private async Task LoadStationsAsync()
        {
            var parameters = new Dictionary<string, string>
            {
                { "options", "keyValues" },
                { "type", EntityType.Station },
            };

            var url = $"{EntityUrl}?{BuildParams(parameters)}";
            var response = await _client.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
                var entities = await response.Content.ReadAsObjectAsync<List<Station>>();
                foreach (var entity in entities)
                {
                    Stations.TryAdd(entity.Name, entity);
                }
            }
            catch (HttpRequestException)
            { }
        }

        private static string BuildParams(Dictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(o => $"{o.Key}={o.Value}"));
        }

        private async Task<string> TryAddStationAsync(string name, string productRef, string breakDuration, string idealDuration, int totalProductCount)
        {
            var result = Stations.TryGetValue(name, out Station station);
            if (!result)
            {
                station = new Station
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    ProductionBreakDuration = breakDuration,
                    ProductionIdealDuration = idealDuration,
                    TotalProductCount = totalProductCount,
                    RefProduct = productRef
                };
                Stations.TryAdd(name, station);

                var content = new StringContent(JsonSerializer.Serialize(station), System.Text.Encoding.UTF8, "application/json");
                await _client.PostAsync(EntityUrl, content);
            }

            return station.GetFullId();
        }

        private async Task<string> TryAddProductAsync(string name)
        {
            var result = Products.TryGetValue(name, out Product product);
            if (!result)
            {
                product = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name
                };
                Products.TryAdd(name, product);

                var content = new StringContent(JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json");
                await _client.PostAsync(EntityUrl, content);
            }

            return product.GetFullId();
        }

        #endregion
    }
}
