using System.Reflection;
using System.Runtime;

namespace PrimeWeb.Types;

public class HpApp
{
	private bool IsSystemApp;

	public string Name { get; set; }
	public int Appsize { get; private set; }
	public Dictionary<string, string> Files { get; set; }

	private byte[] contents;

	public string SvgIcon { get; private set; }

	public byte[] Content { get { return this.contents;  } set { this.contents = value;  } }
	public HpApp(byte[] data)
	{
		ParseByteData(data);
		FindIcon();
		if (this.Name.StartsWith("&"))
		{
			this.IsSystemApp = true;
			this.Name = this.Name.Substring(1);
		}
		else
		{
			this.IsSystemApp = false;
		}
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

				return;
			}
			catch (NullReferenceException e)
			{
				Console.WriteLine("The property does not exist in MyClass." + e.Message);
			}


		}

		this.SvgIcon = HpIcons.USR;
	}
}

