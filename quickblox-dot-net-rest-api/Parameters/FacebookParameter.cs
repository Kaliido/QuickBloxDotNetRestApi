using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kaliido.QuickBlox.Parameters
{
        [JsonObject]
    public class FacebookParameter
    {
        [JsonProperty(PropertyName = "facebook_id")]
        public string FacebookId { get; set; }
    }
}
