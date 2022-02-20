using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Utility
{
	public static class DbgTools
	{
		public static void PrintPacket(ReadOnlySpan<byte> data, int linesize = 32, int maxlines = int.MaxValue, int msg = -1)
		{
			int index = 0;
			int linecounter = 0;

			string id = msg > 0 ? $"message id: {msg}" : $"";

			Console.WriteLine($"# -- # {id}");
			while (index < data.Length && linecounter < maxlines)
			{

				ReadOnlySpan<byte> buffer;
				string line = "";
				if (index + linesize < data.Length)
					buffer = data.Slice(index, linesize);
				else
					buffer = data.Slice(index);

				line = Convert.ToHexString(buffer);
				line += "\t|\t";
				line += Encoding.UTF8.GetString(buffer);
				index+= linesize;
				Console.WriteLine(line);
				linecounter++;
			}
			Console.WriteLine(" -- ");
		}
	}
}
