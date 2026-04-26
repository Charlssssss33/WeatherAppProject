using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrnProjectP511.Models
{
    public class GeocodingResults
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        
        [JsonProperty("country")]
        public string Country {  get; set; }
    }
}
