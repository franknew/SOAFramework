using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroService.Library.SDK;
using Newtonsoft.Json;

namespace MicroService.Library.SDK
{
    public class BaseResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "jsonrpc")]
        public string JsonRpc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "error")]
        public JsonRpcException Error { get; set; }

        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
    }
}
