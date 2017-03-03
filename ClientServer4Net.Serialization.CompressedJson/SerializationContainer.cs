using System;
using Newtonsoft.Json;

namespace ClientServer4Net.Serialization.CompressedJson
{
    public class SerializationContainer
    {
        [JsonProperty("type")]
        public Type MessageType { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
