namespace ClientServer4Net.Messages
{
    internal class SystemNope : IMessage
    {
        private SystemNope() { }
        public static SystemNope Instance => new SystemNope();
    }
}
