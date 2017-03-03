using ClientServer4Net.Messages;

namespace ClientServer4Net.Serialization.CompressedJson.Tests.Messages
{
    public class SimpleMessage : IMessage
    {
        public string Field1 { get; set; }
        public int Field2 { get; set; }
    }
}
