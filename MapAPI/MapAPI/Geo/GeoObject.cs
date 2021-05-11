using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapAPI.Geo
{
    public partial class GeoObject
    {
        [JsonProperty("Point")]
        public Point Point { get; set; }
    }
}
