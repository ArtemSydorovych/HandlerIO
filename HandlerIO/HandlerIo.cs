using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HandlerIO
{
    public static class HandlerIo
    {
        private static readonly ConcurrentQueue<string> Queue = new();
        private static bool _doneEnqueueing = false;

        public static async Task<Stream> ReadUntil(StreamReader reader)
        {
            byte prevFlag = 2;
            var memStream = new MemoryStream();
            var task = new Task(() => Write(memStream));
            task.Start();
            
            while (await reader.ReadLineAsync() is { } line)
            {
                var subs = line.Split(':');
                var flag =  byte.Parse(subs[1]);
                if (flag != prevFlag)
                {
                    Queue.Enqueue(line);
                }

                prevFlag = flag;
            }
            
            Volatile.Write(ref _doneEnqueueing, true);
            task.Wait();
            return memStream;
        }

        private static void Write(Stream stream)
        {
            var writer = new StreamWriter(stream);
            Console.WriteLine("Worker starts");
            do
            {
                while (Queue.TryDequeue(out var line))
                { 
                    writer.WriteAsync(line + "\n");
                    writer.FlushAsync();
                }

                SpinWait.SpinUntil(() => Volatile.Read(ref _doneEnqueueing) || (Queue.Count > 0));
            } while (!Volatile.Read(ref _doneEnqueueing) || (Queue.Count > 0));

            Console.WriteLine("Worker is stopping.");
        }
    }
}