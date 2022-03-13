namespace PrimeWeb.Files;

public class HP_List : HP_Obj
{
	public HP_List() : base(4) { base.Type = Tags.LIST; }

	public List<HP_Obj> Items { get; set; }
	
	
}
