using System;
using ClientServer4Net.Client.Handler;

namespace ClientServer4Net.Server.Handler
{
    public class FactoryServerHandler<T> : AbstractServerHandler where T : AbstractClientHandler
    {
        public override AbstractClientHandler CreateClientHandler()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
