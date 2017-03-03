using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ClientServer4Net.Messages;

namespace ClientServer4Net.Client.Handler
{
    public abstract class AbstractClientHandler
    {
        private readonly ConcurrentQueue<IMessage> _outMessageQueue;
        private readonly LinkedList<Func<IMessage, bool>> _handlers;

        protected AbstractClientHandler()
        {
            _outMessageQueue = new ConcurrentQueue<IMessage>();
            _handlers = new LinkedList<Func<IMessage, bool>>();
        }

        public void Become(Action become)
        {
            _handlers.Clear();
            become();
        }

        public void Receive<T>(Action<T> handler) where T : IMessage
        {
            _handlers.AddLast(message =>
            {
                if (!(message is T)) return false;
                handler((T) message);
                return true;
            });
        }

        public void ReceiveAny(Action<IMessage> handler)
        {
            _handlers.AddLast(message =>
            {
                handler(message);
                return true;
            });
        }

        public void Send(IMessage message)
        {
            _outMessageQueue.Enqueue(message);
        }

        public virtual void PreStart()
        {
            
        }

        public virtual void PostStop()
        {
            
        }

        internal bool TryGetMessage(out IMessage message)
        {
            return _outMessageQueue.TryDequeue(out message);
        }

        internal bool Handle(IMessage message)
        {
            if (message is SystemNope) return false;
            return _handlers.Any(handler => handler(message));
        }
    }
}
