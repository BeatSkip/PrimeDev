using System.Reflection;
using System.Runtime;

namespace PrimeWeb.Types;

public class HpApp
{
	public string Name { get; set; }
	public int Appsize { get; private set; }
	public Dictionary<string, string> Files { get; set; }

	private byte[] contents;

	public string SvgIcon { get; private set; }

	public HpApp(byte[] data)
	{
		ParseByteData(data);
		FindIcon();
	}

	private void ParseByteData(byte[] sourcedata)
	{
		var header = HpFileParser.ParseFileHeader(sourcedata);
		this.contents = header.Contents;
		this.Name = header.Name;
		this.Appsize = (int)header.Size;

	}

	private void FindIcon()
	{
		if (HpIcons.AppTitles.ContainsKey(Name))
		{
			try
			{
				// Get the Type object corresponding to MyClass.
				Type myType = typeof(HpIcons);
				// Get the PropertyInfo object by passing the property name.
				PropertyInfo myPropInfo = myType.GetProperty(HpIcons.AppTitles[Name]);
				this.SvgIcon = (string)myPropInfo.GetValue(null, null);
			}
			catch (NullReferenceException e)
			{
				Console.WriteLine("The property does not exist in MyClass." + e.Message);
			}


		}
	}
}

