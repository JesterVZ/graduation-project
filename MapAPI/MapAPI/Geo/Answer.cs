using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapAPI.Geo
{
    public partial class Answer
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
    }
}
