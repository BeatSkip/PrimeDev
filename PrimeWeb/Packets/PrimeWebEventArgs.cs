using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Packets
{
	public class AppEventArgs : EventArgs
	{
		public string Name { get; set; }
		public HpAppDir App { get; set; }
	}

	public class ChatEventArgs : EventArgs
	{
		public DateTime Date { get; set; }
		public string Message { get; set; }
	}

	public class FileEventArgs : EventArgs
	{
		public string Data { get; set; }
	}

	public class BackupEventArgs : EventArgs
	{
		public HpCalcContents Content { get; set; }
	}

	public class CalcInfoEventArgs : EventArgs
	{
		public HpInfos Content { get; set; }
	}


}
