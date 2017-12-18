using SOAFramework.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AustinHarris.JsonRpc
{
    /// <summary>
    /// Represents a Json Rpc Response
    /// </summary>
    [XmlRoot("Response")]
    [JsonObject(MemberSerialization.OptIn, Title = "Response")]
    public class JsonResponse
    {
        [XmlElement("jsonrpc")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "jsonrpc")]
        public string JsonRpc { get { return "2.0"; } }

        [XmlElement("result")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "result")]
        public object Result { get; set; }

        [XmlElement("error")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "error")]
        public JsonRpcException Error { get; set; }

        [XmlElement("id")]
        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }

        [XmlElement("success")]
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
    }

    /// <summary>
    /// Represents a Json Rpc Response
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [XmlRoot("Response")]
    public class JsonResponse<T>
    {
        [XmlElement("success")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "jsonrpc")]
        public string JsonRpc { get { return "2.0"; } }

        [XmlElement("result")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "result")]
        public T Result { get; set; }

        [XmlElement("error")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "error")]
        public JsonRpcException Error { get; set; }

        [XmlElement("id")]
        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
    }
}
