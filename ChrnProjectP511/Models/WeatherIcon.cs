using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrnProjectP511.Models
{
    public class WeatherIcon
    {
        public int Id { get; set; }
        public int WeatherCode { get; set; }
        public string Description { get; set; }
        public string Emoji { get; set; }
    }
}
