using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChrnProjectP511.Models
{
    public class WeatherData
    {
        public string CityName { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public string WeatherCode { get; set; }
        public string WeatherDiscription { get; set; }
        public string LastUpdated { get; set; }
        public string Icon { get; set; }
    }
}
