using System;

namespace ClientServer4Net.Configuration
{
    // ReSharper disable once InconsistentNaming
    public class CSConfiguration
    {
        public virtual string Host { get; set; }
        public virtual int Port { get; set; }
        public virtual Type SerializerType { get; set; }
    }
}
