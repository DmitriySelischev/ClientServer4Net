namespace ClientServer4Net.Messages
{
    internal class SystemTearDown : IMessage
    {
        private SystemTearDown() { }
        public static SystemTearDown Instance => new SystemTearDown();
    }
}
