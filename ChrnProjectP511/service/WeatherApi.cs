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

        public async Task<WeatherData?> GetWeatherByCityAsync(string cityName)
        {
            try
            {
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

                System.Diagnostics.Debug.WriteLine($"Координаты: {lat}, {lon}");

                string weatherUrl = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";
                System.Diagnostics.Debug.WriteLine($"URL: {weatherUrl}");

                string weatherJson = await client.GetStringAsync(weatherUrl);
                JObject weatherData = JObject.Parse(weatherJson);

                double temp = (double)weatherData["current_weather"]["temperature"];
                double wind = (double)weatherData["current_weather"]["windspeed"];
                int code = (int)weatherData["current_weather"]["weathercode"];

                return new WeatherData
                {
                    CityName = realName,
                    Temperature = temp,
                    WindSpeed = wind,
                    Humidity = 50,
                    Pressure = 1013,
                    WeatherCode = code.ToString(),
                    WeatherDiscription = GetDescription(code),
                    Icon = GetIcon(code),
                    LastUpdated = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }

        private string GetDescription(int code)
        {
            if (code == 0) return "Ясно";
            if (code == 1) return "Облачно";
            if (code == 2) return "Переменная облачность";
            if (code == 3) return "Пасмурно";
            if (code == 45) return "Туман";
            if (code == 61) return "Дождь";
            if (code == 71) return "Снег";
            return "Неизвестно";
        }

        private string GetIcon(int code)
        {
            if (code == 0) return "☀️";
            if (code == 1) return "⛅";
            if (code == 2) return "☁️";
            if (code == 3) return "☁️";
            if (code == 45) return "🌫️";
            if (code == 61) return "🌧️";
            if (code == 71) return "❄️";
            return "❓";
        }
    }
}
