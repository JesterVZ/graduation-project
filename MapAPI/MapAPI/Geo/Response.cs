using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapAPI.Geo
{
    public partial class Response
    {
        [JsonProperty("GeoObjectCollection")]
        public GeoObjectCollection GeoObjectCollection { get; set; }
    }
}
