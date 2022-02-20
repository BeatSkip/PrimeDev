using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Types
{
	public class HpApp
	{
		public string Name { get; set; }
		public int Appsize { get { return sourcedata.Length; } }
		public Dictionary<string, string> Files { get; set; }

		private byte[] sourcedata;


		public HpApp(string name, byte[] data)
		{
			Name = name;
			sourcedata = data;
		}

		private void ParseByteData()
		{

		}
	}
}
