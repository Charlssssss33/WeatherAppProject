using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace ChrnProjectP511.Models
{
    public class WeatherApiResponse
    {
        [JsonProperty("current_weather")]
        public CurrentWeather CurrentWeather { get; set; } = new();

        [JsonProperty("hourlydata")]
        public HourlyData Hourly { get; set; } = new();
    }
}
