using System;
using ClientServer4Net.Messages;

namespace ClientServer4Net.Serialization
{
    public interface ISerializer : IDisposable
    {
        byte[] Serialize(IMessage message);
        IMessage Deserialize(byte[] data);
    }
}