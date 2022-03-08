using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Files
{
	public class HP_Integer : HP_Obj
	{
		public HP_Integer(): base(12) { base.Type = Tags.INT; }
		byte numbits { get; set; } // rage of [-64,64]. Negative values indicated a signed value
		uint padding { get; set; }
		ulong data { get; set; }

		public long Value { get; set; }
	};
}
