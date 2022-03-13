using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Files
{
	public class HP_Note : HP_File
	{
		public string Text { 
			get { return Encoding.UTF8.GetString(base.Content as byte[]); } 
			set { base.Content = Encoding.UTF8.GetBytes(value); } 
		}
		public HP_Note(byte[] data) : base(data) {
		}

		//public HP_Note(HP_File parent)
		//{
		//	base.CRC = parent.CRC;
		//	base.Name = parent.Name;
		//	base.Content = parent.Content;
		//	base.Type = parent.Type;
		//	Console.WriteLine($"Castdd down file to note! {base.Name}");
		//	Console.WriteLine($"content:\n{Text}");
		//}


	}
}
