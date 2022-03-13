using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Files
{
	public class HP_App : HP_File
	{
		public BaseHpApp BaseApp { get; set; }
		

		public HP_App(byte[] data) : base(data)
		{
			var cnt = base.Content as byte[];
			BaseApp = (BaseHpApp)cnt[24];

			Console.WriteLine($"Cast down App: {base.Name} - {BaseApp.ToString()}");
			if(BaseApp == BaseHpApp.Finance || BaseApp == BaseHpApp.TriangleSolver)
				DbgTools.PrintPacket(base.Content as byte[]);
		}
	}
}
