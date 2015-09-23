using Kaliido.QuickBloxDotNet.Api.Extensions;
using Newtonsoft.Json;
using RestSharp.Serializers;
using System.Collections.Generic;

namespace Kaliido.QuickBlox
{
    public class NewtonsoftSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
                                                   {
                                                       NullValueHandling =
                                                           NullValueHandling.Ignore
                                                   };
        public NewtonsoftSerializer(JsonSerializerSettings settings = null)
        {
            if (settings != null)
                _settings = settings;
            this.ContentType = "application/json";
          //  settings.Converters.Add(new ListCompactionConverter());
        }
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
                
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}