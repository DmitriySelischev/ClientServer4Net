using ClientServer4Net.Messages;

namespace Demo.Shared
{
    public class MessageToClient : IMessage
    {
        public string ClientMessage { get; set; }
    }
}
