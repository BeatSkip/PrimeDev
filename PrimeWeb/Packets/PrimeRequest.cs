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

	public PrimeRequest(PrimeCommand Cmd, byte[] data)
	{
		content.Add(Cmd);
		content.Add(PrimeCommands.DefaultParams[Cmd]);
		content.AddRange(data);

	}

	public PrimeRequest(byte Cmd, byte param)
	{
		content.Add(Cmd);
		content.Add(param);

	}

	

	public byte[] Generate()
	{
		return content.ToArray();
	}
}


