using System;
using ClientServer4Net.Configuration;
using ClientServer4Net.Messages;

namespace ClientServer4Net.Serialization
{
    public abstract class SerializationContext : IDisposable
    {
        private readonly ISerializer _serializer;

        protected SerializationContext(CSConfiguration configuration)
        {
            _serializer = (ISerializer) Activator.CreateInstance(configuration.SerializerType);
        }

        protected IMessage Deserialize(string msg)
        {
            var data = Convert.FromBase64String(msg);
            return _serializer.Deserialize(data);
        }

        protected string Serialize(IMessage msg)
        {
            var data = _serializer.Serialize(msg);
            return Convert.ToBase64String(data);
        }

        public virtual void Dispose()
        {
            _serializer?.Dispose();
        }
    }
}
