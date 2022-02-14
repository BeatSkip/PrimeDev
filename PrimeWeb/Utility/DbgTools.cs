﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Utility
{
	public static class DbgTools
	{
		public static void PrintPacket(byte[] data, int linesize = 16)
		{
			int index = 0;
			
			while(index < data.Length)
			{

				byte[] buffer;
				string line = "";
				if (index + linesize < data.Length)
					buffer = data.SubArray(index, linesize);
				else
					buffer = data.SubArray(index);

				line = BitConverter.ToString(buffer).Replace("-", " ");
				line += "\t|\t";
			    line += System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
				index+= linesize;
				Console.WriteLine(line);
			}
		}
	}
}