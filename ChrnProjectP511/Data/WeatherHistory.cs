using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrnProjectP511.Data
{
    public class WeatherHistory
    {
        public int Id { get; set; }
        public string CityName { get; set; } = "";
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public int Humidity { get; set; }
        public string WeatherDescription { get; set; } = "";
        public DateTime SearchedAt { get; set; }
    }
}
