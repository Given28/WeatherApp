using System.Net.Http.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "10aaa563983740eb498522286ff6f536";
        private const string ApiBaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{ApiBaseUrl}?q={city}&appid={ApiKey}&units=metric");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<WeatherData>();
                }

                throw new Exception($"Failed to get weather data: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting weather data: {ex.Message}");
            }
        }

        public async Task<WeatherData> GetWeatherByLocationAsync(double latitude, double longitude)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{ApiBaseUrl}?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<WeatherData>();
                }

                throw new Exception($"Failed to get weather data: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting weather data: {ex.Message}");
            }
        }
    }
}