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
		public List<HP_Note> Notes { get; set; } = new List<HP_Note>();
		public List<HpExamnMode> ExamModes { get; set; } = new List<HpExamnMode>();
		public List<HpAppDir> Apps { get; set; } = new List<HpAppDir>();

		public List<HP_App> Appx { get; set; } = new List<HP_App>();

		public void AddFile(byte[] data)
		{
			var file = new HP_File(data);
			switch (file.Type)
			{
				case PrimeDataTypes.SETTINGS:
					SettingsAdded(file);
					break;
				case PrimeDataTypes.LIST:
					break;
				case PrimeDataTypes.MATRIX:
					break;
				case PrimeDataTypes.PRGM:
					break;
				case PrimeDataTypes.NOTE:
					var note = file as HP_Note;
					Notes.Add(note);
					break;
				case PrimeDataTypes.TESTMODECONFIG:
					break;
				case PrimeDataTypes.APP:
					Console.WriteLine($"APP Detected!");
					var app = new HP_App(file);
					Appx.Add(app);
					break;
			}

		}

		private void SettingsAdded(HP_File file)
		{
			switch (file.Name)
			{
				case "calc.hpsettings":
					break;
				case "cas.hpsettings":
					break;
				case "settings":
					break;
			}
		}

	}
}
