using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Utility
{
	internal class BinaryStreamExtensions
	{
	}

    public static class StreamExtensions
    {
        public static int ReadUntil(this BinaryReader reader, params byte[] sequence)
        {
            int bytesread = 0;
            Queue<byte> buffer = new Queue<byte>(sequence.Length);
            
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                bytesread++;
                var bt = reader.ReadByte();
                buffer.Enqueue(bt);
                if (bt != sequence[sequence.Length - 1])
                    continue;

                if (buffer.SequenceEqual(sequence))
                {
                    return bytesread;
                }
            }

            return -1;
        }

        public static IEnumerable<long> ScanAOB(this Stream stream, params byte?[] aob)
        {
            long position;
            byte[] buffer = new byte[aob.Length - 1];

            while ((position = stream.Position) < stream.Length)
            {
                if (stream.ReadByte() != aob[0]) continue;
                if (stream.Read(buffer, 0, aob.Length - 1) == 0) continue;

                if (buffer.Cast<byte?>().SequenceEqual(aob.Skip(1), new AobComparer()))
                {
                    yield return position;
                }
            }
        }

        private class AobComparer : IEqualityComparer<byte?>
        {
            public bool Equals(byte? x, byte? y) => x == null || y == null || x == y;
            public int GetHashCode(byte? obj) => obj?.GetHashCode() ?? 0;
        }
    }
}
