using ClientServer4Net.Messages;

namespace Demo.Shared
{
    public class MessageToServer : IMessage
    {
        public string ServerMessage { get; set; }
    }
}
