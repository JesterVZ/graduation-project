using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapAPI.Geo
{
    public partial class GeoObjectCollection
    {
        [JsonProperty("featureMember")]
        public FeatureMember[] FeatureMember { get; set; }
    }
}
