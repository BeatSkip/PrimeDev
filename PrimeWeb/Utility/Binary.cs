using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Utility
{
	internal  static class Binary
	{
		public static string GetBCD(ulong data)
		{
			var tmp = data;
			var sb = new StringBuilder();
			int shiftcount = 56;
			for (int i = 0; i < 12; i++)
			{
				int x = (int)((tmp >> shiftcount) & (ulong)0x000000000000000F);
				sb.Append(x.ToString());
				shiftcount -= 4;
			}
			return sb.ToString();
		}

		public static double HpBCD(ulong data)
		{
			var tmp = data;
			int shiftcount = 56;
			double scalar = 1.0;
			double result = 0;
			for (int i = 0; i < 12; i++)
			{
				var x = ((double)(int)((tmp >> shiftcount) & (ulong)0x000000000000000F));
				result += (x * scalar);
				shiftcount -= 4;
				scalar = scalar / 10.0;
			}

			return result;
		}
	}
}
