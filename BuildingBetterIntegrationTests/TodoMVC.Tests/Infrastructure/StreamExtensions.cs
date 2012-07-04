using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static byte[] ReadToEnd(this Stream stream)
        {
            var buffer = new byte[32768];
            using (var memoryStream = new MemoryStream())
            {
                while (true)
                {
                    var read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return memoryStream.ToArray();
                    memoryStream.Write(buffer, 0, read);
                }
            }
        }
    }
}
