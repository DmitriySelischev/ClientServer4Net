using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ClientServer4Net.Client.Handler;
using ClientServer4Net.Configuration;
using ClientServer4Net.Messages;
using ClientServer4Net.Serialization;
using ClientServer4Net.Server.Handler;

namespace ClientServer4Net.Server
{
    public class Server : SerializationContext
    {
        private readonly CSConfiguration _configuration;
        private bool _isConnected;
        private Thread _thread;

        public Server(CSConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _isConnected = false;
        }

        public void Start(AbstractServerHandler handler, bool lockThread=true)
        {
            if (lockThread)
            {
                ProcessInternally(handler);
            }
            else
            {
                _thread = new Thread(obj => ProcessInternally((AbstractServerHandler) obj));
                _thread.Start(handler);
            }
        }

        private void ProcessInternally(AbstractServerHandler serverHandler)
        {
            TcpListener listener = null;
            try
            {
                _isConnected = true;
                var addresses = Dns.GetHostAddresses(_configuration.Host);
                listener = new TcpListener(addresses[0], _configuration.Port);
                listener.Start();
                while (_isConnected)
                {
                    var acceptedTcpClient = listener.AcceptTcpClient();
                    var thread = new Thread(obj =>
                    {
                        AbstractClientHandler handler = null;
                        TcpClient client = null;
                        try
                        {
                            handler = serverHandler.CreateClientHandler();
                            handler.PreStart();
                            client = (TcpClient)obj;
                            var stream = client.GetStream();
                            var reader = new StreamReader(stream);
                            var writer = new StreamWriter(stream);
                            while (_isConnected)
                            {
                                IMessage msgToSend;
                                while (handler.TryGetMessage(out msgToSend))
                                {
                                    writer.WriteLine(Serialize(msgToSend));
                                    writer.Flush();
                                }
                                writer.WriteLine(Serialize(SystemGetMessage.Instance));
                                writer.Flush();
                                var msg = Deserialize(reader.ReadLine());
                                handler.Handle(msg);
                            }
                            
                        }
                        catch (Exception)
                        {
                            //Ignored. Just close close connection.
                        }
                        finally
                        {
                            client?.Close();
                            handler?.PostStop();
                        }
                    });
                    thread.Start(acceptedTcpClient);
                }
            }
            finally
            {
                listener?.Stop();
            }
        }
    }
}
