using ClientServer4Net.Client.Handler;

namespace ClientServer4Net.Server.Handler
{
    public abstract class AbstractServerHandler
    {
        public abstract AbstractClientHandler CreateClientHandler();
    }
}
