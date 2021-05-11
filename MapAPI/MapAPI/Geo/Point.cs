using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapAPI.Geo
{
    public partial class Point
    {
        [JsonProperty("pos")]
        public string Pos { get; set; }
    }
}
