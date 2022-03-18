using PrimeWeb.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Utility
{

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

        public static long BytesToGo(this BinaryReader reader)
		{
            return (reader.BaseStream.Length - reader.BaseStream.Position);
		}



        public static string ReadUnicodeString(this BinaryReader reader, int length)
        {
            return Conversion.DecodeTextData(reader, length);
        }

        public static HP_Real ReadHpReal(this BinaryReader reader)
		{
            HP_Real hp = new HP_Real();
            return hp;
		}

        
        public static int ReadLeInt32(this BinaryReader reader)
		{
            return (int)Conversion.ReadLittleEndianBytes(reader);
		}

        public static uint ReadLeUint32(this BinaryReader reader)
        {
            return Conversion.ReadLittleEndianBytes(reader);
        }

        public static int ReadBeInt32(this BinaryReader reader)
        {
            return (int)Conversion.ReadLittleEndianBytes(reader);
        }

        public static uint ReadBeUint32(this BinaryReader reader)
        {
            return Conversion.ReadLittleEndianBytes(reader);
        }

        public static uint ReadUint32(this BinaryReader reader)
		{
            var bytes = reader.ReadBytes(4);
            var LE = Conversion.ReadLittleEndianBytes(bytes);
            var BE = Conversion.ReadBigEndianBytes(bytes);
            return LE < BE ? LE : BE;
		}

        public static int ReadInt32(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(4);
            var LE = (int)Conversion.ReadLittleEndianBytes(bytes);
            var BE = (int)Conversion.ReadBigEndianBytes(bytes);
            return LE < BE ? LE : BE;
        }
        

    }

    
}
