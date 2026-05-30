using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Metadata;
using ChrnProjectP511.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace ChrnProjectP511.service
{

    public class WeatherApi
    {
            private static readonly HttpClient client = new HttpClient();
            private readonly DatabaseService _dbService = new DatabaseService();

            public async Task<WeatherData?> GetWeatherByCityAsync(string cityName)
            {
                try
                {
                    // Получаем координаты
                    string geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={cityName}&count=1";
                    string geoJson = await client.GetStringAsync(geoUrl);
                    JObject geoData = JObject.Parse(geoJson);

                    if (geoData["results"] == null || geoData["results"].Count() == 0)
                    {
                        return null;
                    }

                    double latRaw = (double)geoData["results"][0]["latitude"];
                    double lonRaw = (double)geoData["results"][0]["longitude"];
                    string realName = (string)geoData["results"][0]["name"];

                    string lat = latRaw.ToString(CultureInfo.InvariantCulture);
                    string lon = lonRaw.ToString(CultureInfo.InvariantCulture);

                    // Получаем погоду
                    string weatherUrl = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";
                    string weatherJson = await client.GetStringAsync(weatherUrl);
                    JObject weatherData = JObject.Parse(weatherJson);

                    double temp = (double)weatherData["current_weather"]["temperature"];
                    double wind = (double)weatherData["current_weather"]["windspeed"];
                    int code = (int)weatherData["current_weather"]["weathercode"];

                    // Получаем иконку ИЗ БАЗЫ ДАННЫХ
                    var iconFromDb = await _dbService.GetIconByCodeAsync(code);

                    string description = iconFromDb?.Description ?? "Неизвестно";
                    string emoji = iconFromDb?.Emoji ?? "❓";

                    var weather = new WeatherData
                    {
                        CityName = realName,
                        Temperature = temp,
                        WindSpeed = wind,
                        Humidity = 50,
                        Pressure = 1013,
                        WeatherCode = code.ToString(),
                        WeatherDescription = description,
                        Icon = emoji,
                        LastUpdated = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                    };

                    // Сохраняем в историю
                    await _dbService.SaveToHistoryAsync(weather);

                    return weather;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                    return null;
                }
        }
    }
}
