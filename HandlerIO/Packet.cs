using System;

namespace HandlerIO
{
    public struct Packet
    {
        public ulong Timestamp { get; }
        public byte  Flag { get; }
        public Packet(string inStr)
        {
            var subs = inStr.Split(':');
            Timestamp = ulong.Parse(subs[0]);
            Flag =  byte.Parse(subs[1]);
        }
    }
}