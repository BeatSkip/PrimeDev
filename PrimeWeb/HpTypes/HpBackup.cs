using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.HpTypes
{
	[Serializable]
	public class HpBackup
	{
		public HpBackup()
		{
			this.Date = DateTime.Now;
		}
		public DateTime Date { get; set; }
		public HpCalcSettings CALCSettings { get; set; }
		public HpCasSettings CASSettings { get; set; }
		public HpVars CALChpvars { get; set; }

		public List<HpList> Lists { get; set; } = new List<HpList>();
		public List<HpMatrix> Matrices { get; set; } = new List<HpMatrix>();
		public List<HpProgram> Programs { get; set; } = new List<HpProgram>();
		public List<HpNote> Notes { get; set; } = new List<HpNote>();
		public List<HpExamnMode> ExamModes { get; set; } = new List<HpExamnMode>();
		public List<HpApp> Apps { get; set; } = new List<HpApp>();

	}
}
