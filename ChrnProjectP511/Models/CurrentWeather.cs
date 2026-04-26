using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChrnProjectP511.Models
{
    public class CurrentWeather
    {
        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("windspeed")]
        public double WindSpeed {  get; set; }

        [JsonProperty("weathercode")]
        public int WeatherCode { get; set; }

        [JsonProperty("time")]
        public string Time {  get; set; }
    }
}
