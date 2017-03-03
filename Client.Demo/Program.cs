using System;
using System.Threading;
using ClientServer4Net.Client.Handler;
using ClientServer4Net.Configuration;
using ClientServer4Net.Serialization.CompressedJson;
using Demo.Shared;

namespace Client.Demo
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Thread.Sleep(1000);
            var config = new CSConfiguration
            {
                Host = "192.168.1.11",
                Port = 3456,
                SerializerType = typeof(Serializer)
            };
            var client = new ClientServer4Net.Client.Client(config);
            client.Start(new ClientHandler());
            Console.WriteLine("Client stop");
            Console.ReadLine();
        }
    }

    public class ClientHandler : AbstractClientHandler
    {
        public ClientHandler()
        {
            Become(Ready);
        }

        public void Ready()
        {
            Receive<MessageToClient>(msg =>
            {
                Console.WriteLine($"Client: {msg.ClientMessage}");
                Send(new MessageToServer {ServerMessage = "Server message!!!"});
            });
        }
    }
}
