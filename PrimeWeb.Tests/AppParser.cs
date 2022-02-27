using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Tests;

public class AppParser
{
	byte[] sourcedata;

	public AppParser(byte[] data)
	{
		this.sourcedata = data;


	}


	public (byte[] hpapp, byte[] files) SplitHeader(byte[] data)
	{
		var sequence = new byte[] { 0x00, 0x00, 0x00, 0x02, 0x00, 0x00 };

		var length = Tools.Search(data, sequence);

		if(length == -1)
		{
			Console.WriteLine("did not find sequence! returning all data as header!");
			return (data, new byte[0]);
		}

		using(var ms = new MemoryStream(data))
		using(var reader = new BinaryReader(ms))
		{

			var header = reader.ReadBytes(length);
			var splitter = reader.ReadBytes(sequence.Length);

			if (splitter != sequence)
				Console.WriteLine("splitter value is not the same!");

			var content = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
			return (header,content);

		}

	}






}
