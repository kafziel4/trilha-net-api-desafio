using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrilhaApiDesafioTests.Helpers
{
    public static class ResponseHelper
    {
        public static async Task<T?> GetResponseBody<T>(HttpResponseMessage response)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            return await response.Content.ReadFromJsonAsync<T>(jsonOptions);
        }
    }
}
