﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Utility
{
	public static class DbgTools
	{
		public static void PrintPacket(ReadOnlySpan<byte> data, int linesize = 16, int maxlines = int.MaxValue)
		{
			int index = 0;
			int linecounter = 0;
			Console.WriteLine("# -- # ");
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
				line += buffer.ToString();
				index+= linesize;
				Console.WriteLine(line);
				linecounter++;
			}
			Console.WriteLine(" -- ");
		}
	}
}
