using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapAPI.Geo
{
    public partial class FeatureMember
    {
        [JsonProperty("GeoObject")]
        public GeoObject GeoObject { get; set; }
    }
}
