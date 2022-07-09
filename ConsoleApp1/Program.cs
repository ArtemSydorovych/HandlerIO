using System;
using System.IO;
using System.Threading.Tasks;
using HandlerIO;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await using var fs = File.Open("/Users/artemsydorovych/RiderProjects/InterviewTaskSM/test.txt",
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            await using var bs = new BufferedStream(fs);
            using var sr = new StreamReader(bs);
            await foreach (var line in HandlerIo.ReadUntil(sr)) 
            {
                Console.WriteLine(line);
            }


        }

        public static Stream GenerateStreamFromStringList(string[] s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            foreach (var str in s)
            {
                writer.Write(str);
                writer.Flush();
            }
            stream.Position = 0;
            return stream;
        }
    }
}