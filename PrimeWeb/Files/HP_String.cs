using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Files
{
	public class HP_String : HP_Obj
	{
		public HP_String(): base(3) { base.Type = Tags.STRING; }

		byte padding;
		byte num_chars;
		char[] data;

		public string Value { get; set; }
	};
}
