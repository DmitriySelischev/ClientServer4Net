using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using ClientServer4Net.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ClientServer4Net.Serialization.CompressedJson
{
    public class Serializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public Serializer()
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Culture = CultureInfo.InvariantCulture,
                Formatting = Formatting.None
            };
        }
        /// <summary>
        ///     Not used
        /// </summary>
        public void Dispose()
        {
        }

        public byte[] Serialize(IMessage message)
        {
            var json = JsonConvert.SerializeObject(message, _settings);
            var container = new SerializationContainer
            {
                MessageType = message.GetType(),
                Data = json
            };
            var serializedContainer = JsonConvert.SerializeObject(container);
            using (var memoryStream = new MemoryStream())
            {
                using (var gzip = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    using (var writer = new StreamWriter(gzip, Encoding.UTF8))
                    {
                        writer.Write(serializedContainer);
                        writer.Flush();
                    }
                }
                return memoryStream.ToArray();
            }
        }

        public IMessage Deserialize(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                using (var gzip = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gzip, Encoding.UTF8))
                    {
                        var serializedContainer = reader.ReadToEnd();
                        var container = JsonConvert.DeserializeObject<SerializationContainer>(serializedContainer,
                            _settings);
                        var json = container.Data;
                        if (typeof(IMessage).IsAssignableFrom(container.MessageType))
                        {
                            return (IMessage) JsonConvert.DeserializeObject(json, container.MessageType, _settings);
                        }
                        return null;
                    }
                }
            }
        }
    }
}
