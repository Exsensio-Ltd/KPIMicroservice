using KPIMicroservice.Models.OEE;
using KPIMicroservice.Serializers;
using KPIMicroservice.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace KPIMicroservice.Utils
{
    public class ContextClient
    {
        #region Fields

        public const string TimeFormat = @"hh\:mm\:ss\.fff";
        private readonly string _entityUrl = "v2/entities";
        private readonly ConcurrentDictionary<string, Product> _products = new();
        private readonly ConcurrentDictionary<string, Station> _stations = new();
        private readonly HttpClientHandler _httpHandler = new();
        private readonly HttpClient _client;
        private static ContextClient _instance = null;
        private static string Url = "";

        #endregion

        #region Constructors

        private ContextClient(string baseUrl)
        {
            _client = new HttpClient(_httpHandler, true)
            {
                BaseAddress = new Uri(baseUrl)
            };

            Task.WaitAll(LoadProductsAsync(), LoadStationsAsync());
        }

        #endregion

        #region Methods

        public static ContextClient GetInstance(string url = "http://localhost:1026")
        {
            if (_instance == null || Url != url)
            {
                Url = url;
                _instance = new ContextClient(url);
            }

            return _instance;
        }

        public async Task CreateEntityAsync(string product, string station, string breakDuration, string idealDuration, int totalProductCount = 0)
        {
            var productId = await TryAddProductAsync(product);
            var stationId = await TryAddStationAsync(station, productId, breakDuration, idealDuration, totalProductCount);

            await UpdateStationMeta(stationId, new Station
            {
                ProductionBreakDuration = breakDuration,
                ProductionIdealDuration = idealDuration,
                TotalProductCount = totalProductCount
            });

            var entity = new OEEMetric
            {
                Id = Guid.NewGuid().ToString(),
                RefStation = stationId,
            };

            var content = new StringContent(JsonSerializer.Serialize(entity), System.Text.Encoding.UTF8, "application/json");
            await _client.PostAsync(_entityUrl, content);
        }

        public async Task UpdateStationMeta(string stationId, Station meta)
        {
            var id = GetFullId(EntityType.Station, stationId);

            var json = JsonSerializer.Serialize(meta, new JsonSerializerOptions
            {
                IgnoreNullValues = true
            });
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_entityUrl}/{id}/attrs", content);
            await response.Content.ReadAsStringAsync();
        }

        public IEnumerable<Product> GetProducts()
        {
            return _products.Select(p => new Product
            {
                Id = p.Value.Id,
                Type = EntityType.Product,
                Name = p.Value.Name,
                Stations = _stations
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
            var id = GetFullId(EntityType.Station, stationId);

            var parametersStation = BuildParams(new Dictionary<string, string>
            {
                { "options", "keyValues" },
                { "type", EntityType.Station },
            });

            var responseStation = await _client.GetAsync($"{_entityUrl}/{id}?{parametersStation}");
            responseStation.EnsureSuccessStatusCode();
            var station = await responseStation.Content.ReadAsObjectAsync<Station>();

            var parametersMetric = BuildParams(new Dictionary<string, string>
            {
                { "options", "keyValues" },
                { "type", EntityType.OEEMetric },
                { "limit", "1000" },
                { "refStation", id }
            });

            var responseMetric = await _client.GetAsync($"{_entityUrl}?{parametersMetric}");
            responseMetric.EnsureSuccessStatusCode();
            var metrics = await responseMetric.Content.ReadAsObjectAsync<IEnumerable<OEEMetric>>();

            station.Metrics = metrics;

            return station;
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

            var url = $"{_entityUrl}?{BuildParams(parameters)}";
            var response = await _client.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
                var entities = await response.Content.ReadAsObjectAsync<List<Product>>();
                foreach (var entity in entities)
                {
                    _products.TryAdd(entity.Name, entity);
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

            var url = $"{_entityUrl}?{BuildParams(parameters)}";
            var response = await _client.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
                var entities = await response.Content.ReadAsObjectAsync<List<Station>>();
                foreach (var entity in entities)
                {
                    _stations.TryAdd(entity.Name, entity);
                }
            }
            catch (HttpRequestException)
            { }
        }

        private static string GetFullId(string type, string id)
        {
            return $"urn:ngsi-ld:{type}:{id}";
        }

        private static string BuildParams(Dictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(o => $"{o.Key}={o.Value}"));
        }

        private async Task<string> TryAddStationAsync(string name, string productRef, string breakDuration, string idealDuration, int totalProductCount)
        {
            var result = _stations.TryGetValue(name, out Station station);
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
                _stations.TryAdd(name, station);

                var content = new StringContent(JsonSerializer.Serialize(station), System.Text.Encoding.UTF8, "application/json");
                await _client.PostAsync(_entityUrl, content);
            }

            return station.GetFullId();
        }

        private async Task<string> TryAddProductAsync(string name)
        {
            var result = _products.TryGetValue(name, out Product product);
            if (!result)
            {
                product = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name
                };
                _products.TryAdd(name, product);

                var content = new StringContent(JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json");
                await _client.PostAsync(_entityUrl, content);
            }

            return product.GetFullId();
        }

        #endregion
    }
}
