using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

public static class ViewStateCompression
{
    private const int BUFFER_SIZE = 65536;
    static int _stateCompression = Deflater.NO_COMPRESSION;

    public static int StateCompression
    {
        get { return _stateCompression; }
        set { _stateCompression = value; }
    }

    public static byte[] Compress(byte[] bytes)
    {
        using (MemoryStream memoryStream = new MemoryStream(BUFFER_SIZE))
        {
            Deflater deflater = new Deflater(StateCompression);
            using (Stream stream = new DeflaterOutputStream(memoryStream, deflater, BUFFER_SIZE))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }
    }

    public static byte[] Decompress(byte[] bytes)
    {
        using (MemoryStream byteStream = new MemoryStream(bytes))
        {
            using (Stream stream = new InflaterInputStream(byteStream))
            {
                using (MemoryStream memory = new MemoryStream(BUFFER_SIZE))
                {
                    byte[] buffer = new byte[BUFFER_SIZE];
                    while (true)
                    {
                        int size = stream.Read(buffer, 0, BUFFER_SIZE);
                        if (size <= 0)
                            break;

                        memory.Write(buffer, 0, size);
                    }
                    return memory.ToArray();
                }
            }
        }
    }
}