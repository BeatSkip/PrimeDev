using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Calculator;

namespace PrimeWeb.Types
{
	public class HpApp
	{
		public string Name { get; set; }
		public int Appsize { get { return sourcedata.Length; } }
		public Dictionary<string, string> Files { get; set; }
		private byte[] sourcedata;

		public string SvgIcon { get; private set; }
		public HpApp(string name, byte[] data)
		{
			Name = name;
			sourcedata = data;
			FindIcon();
		}

		private void ParseByteData()
		{

		}

		private void FindIcon()
		{
			if (this.Name.Contains("Explorer"))
				this.SvgIcon = HpIcons.Explorer;
			else if (this.Name.Contains("Function"))//Parametric
				this.SvgIcon = HpIcons.Function;
			else if (this.Name.Contains("Inference"))
				this.SvgIcon = HpIcons.Inference;
			else if (this.Name.Contains("Parametric"))
				this.SvgIcon = HpIcons.Parametric;
			else if (this.Name.Contains("&Advanced"))
				this.SvgIcon = HpIcons.AdvancedGraphing;
		}
	}
}
