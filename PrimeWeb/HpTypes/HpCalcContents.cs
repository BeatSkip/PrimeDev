using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Files;

namespace PrimeWeb.HpTypes
{
	[Serializable]
	public class HpCalcContents
	{
		public HpCalcContents()
		{
			this.Date = DateTime.Now;
		}
		public DateTime Date { get; set; }
		public HpCalcSettings CALCSettings { get; set; }
		public HpCasSettings CASSettings { get; set; }
		public HpVars CALChpvars { get; set; }

		public List<HP_List> Lists { get; set; } = new List<HP_List>();
		public List<HpMatrix> Matrices { get; set; } = new List<HpMatrix>();
		public List<HpProgram> Programs { get; set; } = new List<HpProgram>();
		public List<HpNote> Notes { get; set; } = new List<HpNote>();
		public List<HpExamnMode> ExamModes { get; set; } = new List<HpExamnMode>();
		public List<HpAppDir> Apps { get; set; } = new List<HpAppDir>();

		public void AddFile(byte[] data)
		{
			var file = new HP_File(data);

			switch (file.Name)
			{

			}
		}

	}
}
