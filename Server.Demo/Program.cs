using System;
using System.Threading;
using System.Threading.Tasks;
using ClientServer4Net.Client.Handler;
using ClientServer4Net.Configuration;
using ClientServer4Net.Serialization.CompressedJson;
using ClientServer4Net.Server.Handler;
using Demo.Shared;

namespace Server.Demo
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var config = new CSConfiguration
            {
                Host = "192.168.1.11",
                Port = 3456,
                SerializerType = typeof(Serializer)
            };
            var server = new ClientServer4Net.Server.Server(config);
            server.Start(new FactoryServerHandler<ClientHandler>());
            Console.WriteLine("Server stop");
            Console.ReadLine();
        }
    }

    public class ClientHandler : AbstractClientHandler
    {
        public ClientHandler()
        {
            Become(Ready);
        }

        public override void PreStart()
        {
            Send(new MessageToClient {ClientMessage = "Fucking message"});
        }

        public void Ready()
        {
            Receive<MessageToServer>(msg =>
            {
                Console.WriteLine($"Server: {msg.ServerMessage}");
                Task.Run(() =>
                {
                    Thread.Sleep(500);
                    Send(new MessageToClient { ClientMessage = "Client message!!!" });
                });
            });
        }
    }
}
