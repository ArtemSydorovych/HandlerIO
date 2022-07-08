using System;
using System.IO;
using System.Text;
using HandlerIO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new string[]
            {
                "1615560000: 1\n",
                "1615560005: 1\n",
                "1615560013: 1\n",
                "1615560018: 1\n",
                "1615560024: 0\n",
                "1615560030: 1\n",
                "1615560037: 0\n",
            };
            var sr = new StreamReader(GenerateStreamFromStringList(test));
            var lines = sr.ReadUntil();
            sr = new StreamReader(lines.Result);
            while (sr.ReadLine() is { } line)
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