using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace HandlerIO
{
    public static class StreamReaderExtensions
    {
        public static async Task<MemoryStream> ReadUntil(this StreamReader reader)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            byte prevFlag = 2;
            while (await reader.ReadLineAsync() is { } line)
            {
                var pct = new Packet(line);
                if (pct.Flag != prevFlag)
                {
                    await writer.WriteAsync(line + "\n");
                    await writer.FlushAsync();
                }
                    
                prevFlag = pct.Flag;
            }

            stream.Position = 0;
            return stream;
        }
    }
}