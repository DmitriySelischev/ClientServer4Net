using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using ClientServer4Net.Client.Handler;
using ClientServer4Net.Configuration;
using ClientServer4Net.Messages;
using ClientServer4Net.Serialization;

namespace ClientServer4Net.Client
{
    public class Client : SerializationContext
    {
        private readonly CSConfiguration _configuration;
        private bool _isConnected;
        private Thread _thread;

        public Client(CSConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _isConnected = false;
        }

        public void Start(AbstractClientHandler handler, bool lockThread=true)
        {
            if (lockThread)
            {
                ProcessInternally(handler);
            }
            else
            {
                _thread = new Thread(obj => ProcessInternally((AbstractClientHandler) obj));
                _thread.Start(handler);
            }
        }

        private void ProcessInternally(AbstractClientHandler handler)
        {
            TcpClient tcpClient = null;
            try
            {
                _isConnected = true;
                handler.PreStart();
                tcpClient = new TcpClient(_configuration.Host, _configuration.Port);
                var stream = tcpClient.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                while (_isConnected)
                {
                    var msg = Deserialize(reader.ReadLine());
                    if (msg is SystemGetMessage)
                    {
                        IMessage messageToSend;
                        writer.WriteLine(handler.TryGetMessage(out messageToSend)
                            ? Serialize(messageToSend)
                            : Serialize(SystemNope.Instance));
                        writer.Flush();
                    }
                    else
                    {
                        handler.Handle(msg);
                    }
                }
            }
            catch (Exception)
            {
               //Ignored. Just close connection 
            }
            finally
            {
                tcpClient?.Close();
                handler.PostStop();
            }
        }

        public void Stop()
        {
            _isConnected = false;
            _thread?.Abort();
        }

        public override void Dispose()
        {
            base.Dispose();
            Stop();
        }
    }
}
