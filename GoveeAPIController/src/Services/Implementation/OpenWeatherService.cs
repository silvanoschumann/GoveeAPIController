using GoveeAPIController.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoveeAPIController.src.Services.Implementation
{
    public class OpenWeatherService
    {
        private readonly HttpClient _httpClient = new();

        private readonly string _apiKey = GoveeApplication.OpenWeatherApiKey;
        private readonly string _city = "Mannheim";
        private readonly string _countryCode = "DE";


        public async Task<DateTimeOffset> GetSunsetAsync()
        {
            string url =
                $"https://api.openweathermap.org/data/2.5/weather" +
                $"?q={_city},{_countryCode}" +
                $"&appid={_apiKey}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            var data = System.Text.Json.JsonSerializer.Deserialize<OpenWeatherResponse>(jsonString);

            if (data.Sys == null)
                throw new InvalidOperationException("Sunset data missing");

            return DateTimeOffset.FromUnixTimeSeconds(data.Sys.Sunset);
        }

    }
}
