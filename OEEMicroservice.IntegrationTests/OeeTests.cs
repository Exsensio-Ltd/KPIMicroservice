using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace OEEMicroservice.IntegrationTests
{
    public class OeeTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public OeeTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task TestGetStationsAsync()
        {
            // Arrange
            var request = "/api/oee/stations";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestAddOeeMetricAsync()
        {
            // Arrange
            var request = "/api/oee/add";

            // Act
            var entity = new
            {
                ProductName = "Test Product",
                StationName = "Test Station",
                ProductionIdealDuration = "00:00:55.231",
                ProductionBreakDuration = "00:00:01.442",
                TotalProductCount = 1
            };
            var content = new StringContent(JsonSerializer.Serialize(entity), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(request, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestUpdateStationMetaAsync()
        {
            // Arrange
            var request = "/api/oee/station/9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb";

            // Act
            var entity = new
            {
                ProductionIdealDuration = "00:00:55.122",
                ProductionBreakDuration = "00:00:01.422",
                TotalProductCount = 2
            };
            var json = JsonSerializer.Serialize(entity);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(request, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestCalculateAsync()
        {
            // Arrange
            var request = "/api/oee/calculate?id=9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb&reportingPeriod=1&type=0";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
