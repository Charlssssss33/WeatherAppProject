using Avalonia.Metadata;
using ChrnProjectP511.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace ChrnProjectP511.service
{
    public class WeatherApi
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<WeatherData?> GetWeatherByCityAsync(string cityName)
        {
            try
            {
                var coordinates = await GetCoordinatesAsync(cityName);
                if (coordinates == null)
                {
                    return null;
                }
                var weather = await GetWeatherByCoordinatesAsync(coordinates.Latitude, coordinates.Longitude);
                if (weather == null)
                {
                    return null;
                }
                weather.CityName = coordinates.Name;
                weather.LastUpdated = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                return weather;
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }
        private async Task<CityCoordinates?> GetCityCoordinatesAsync(string cityName)
        {
            string url = "$https://geocoding-api.open-meteo.com/v1/search?name={cityName}&count=1\";";

           string Json = await _httpClient.GetStringAsync(url);
            
            JObject data = JObject.Parse(Json);

            var firstResult = data["results"]?[0];
            if (firstResult == null)
            {
                return null;
            }

            return new CityCoordinates()
            {
                Name = firstResult["name"]?.ToString() ?? cityName,
                Latitude = (double)firstResult["latitude"],
                Longitude = (double)firstResult["longitude"],
                Country = firstResult["country"]?.ToString() ?? ""
            };
        }
    }
}
