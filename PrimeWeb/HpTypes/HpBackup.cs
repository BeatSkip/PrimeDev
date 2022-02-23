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
		public DateTime Date { get; set; }
		public HpCalcSettings CALCSettings { get; set; }
		public HpCasSettings CASSettings { get; set; }
		public HpVars CALChpvars { get; set; }

		public List<HpList> OTLists { get; set; }
		public List<HpMatrix> OTMatrices { get; set; }
		public List<HpProgram> OTPrograms { get; set; }
		public List<HpNote> OTNotes { get; set; }
		public List<HpExamnMode> OTExamModes { get; set; }
		public List<HpApp> OTApps { get; set; }

	}
}
