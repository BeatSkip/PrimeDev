using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Packets;

public class PrimeRequest : IPayloadGenerator
{

	private List<byte> content = new List<byte>();

	public PrimeRequest(PrimeCommand Cmd)
	{
		content.Add(Cmd);
		content.Add(PrimeCommands.DefaultParams[Cmd]);
	}

	public PrimeRequest(byte Cmd, byte param)
	{
		content.Add(Cmd);
		content.Add(param);

	}

	public PrimeRequest(byte Command, string name)
	{
		
		var namebytes = Conversion.EncodeTextData(name);
		uint length = (uint)namebytes.Length + 4;

		content.Add((byte)Command);
		content.Add((byte)CmdType.Three);//Still unsure what and how about this one... 0x03 or 0x01, haven't noticed a difference yet..
		content.AddRange(Conversion.GetBigEndianBytes(length));//Name Length
		content.Add(0x00);//CRC
		content.Add(0x00);//CRC
		content.AddRange(namebytes);
		

	}

	public byte[] Generate()
	{
		return content.ToArray();
	}
}


