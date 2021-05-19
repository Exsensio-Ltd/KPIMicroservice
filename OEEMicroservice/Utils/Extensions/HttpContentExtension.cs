using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OEEMicroservice.Utils.Extensions
{
    public static class HttpContentExtension
    {
        public static async Task<T> ReadAsObjectAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
