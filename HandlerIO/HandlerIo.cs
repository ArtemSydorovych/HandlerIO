using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HandlerIO
{
    public static class HandlerIo
    {
        public static async IAsyncEnumerable<string> ReadUntil(StreamReader reader)
        {
            byte prevFlag = 2;
            while (await reader.ReadLineAsync() is { } line)
            {
                var subs = line.Split(':');
                var flag =  byte.Parse(subs[1]);
                if (flag != prevFlag)
                {
                    Console.WriteLine("writes");
                    yield return line;
                }

                prevFlag = flag;
            }
        }
    }
}