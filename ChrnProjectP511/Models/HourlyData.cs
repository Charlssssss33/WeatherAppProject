using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrnProjectP511.Models
{
    public class HourlyData
    {
        [JsonProperty("relativehumidity_2m")]
        public List<int> RelativeHumidity { get; set; } = new();

        [JsonProperty("pressure_ms1")]
        public List<double> Pressure { get; set; } = new();
    }
}
