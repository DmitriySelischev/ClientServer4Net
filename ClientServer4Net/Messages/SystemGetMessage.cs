namespace ClientServer4Net.Messages
{
    internal class SystemGetMessage : IMessage
    {
        private SystemGetMessage() { }
        public static SystemGetMessage Instance => new SystemGetMessage();
    }
}
