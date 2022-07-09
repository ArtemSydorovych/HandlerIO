using System;
using System.Collections.Generic;
using System.IO;
using HandlerIO;
using Xunit;

namespace TestIOHandler
{
    public class TestWithMemoryStream
    {
        private readonly string[] _strings =
        {
            "1615560000: 1\n",
            "1615560005: 1\n",
            "1615560013: 1\n",
            "1615560018: 1\n",
            "1615560024: 0\n",
            "1615560030: 1\n",
            "1615560037: 0\n",
            "1615560042: 0\n"
        };
        [Fact]
        public async void TestWithGivenDataExample()
        {
            var list = new List<string>();
            await using var bs = GenerateStreamFromStringList(_strings);
            using var sr = new StreamReader(bs);
            await foreach (var line in HandlerIo.ReadUntil(sr))
            {
                list.Add(line);
            }
            Assert.True(list.Count == 4);
            Assert.Contains("1615560000: 1", list);
            Assert.Contains("1615560024: 0", list);
            Assert.DoesNotContain("1615560005: 1", list);
            Assert.DoesNotContain("1615560042: 0", list);
            
        }
        
        
        private static Stream GenerateStreamFromStringList(string[] s)
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