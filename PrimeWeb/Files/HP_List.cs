namespace PrimeWeb.Files;

public class HP_List : HP_Obj
{
	public uint Count { get; private set; }
	public HP_List() : base(4) { base.Type = Tags.LIST; }


	public HP_List(byte[] data) : base (data) { }

	public HP_List(BinaryReader reader): base(8)
	{

		Console.WriteLine($"Starting list read with {Count} items");
		Items = new List<HP_Obj>();
		for (int i = 0; i < Count; i++)
		{
			Items.Add(HP_Obj.ReadObject(reader));
		}

		Console.WriteLine($"End List Read");
	}

	public List<HP_Obj> Items { get; set; }
	
	
}
