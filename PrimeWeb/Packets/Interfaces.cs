namespace PrimeWeb.Packets;

public interface IPacketPayload
{
	public void ReversePayload(byte[] payload);
	public byte[] GeneratePayload();
	public Type Type { get; }
}

public interface IPayloadGenerator
{
	public byte[] Generate();
}

public interface IPayloadParser
{
	public void ParsePayload(byte[] payload);
}
