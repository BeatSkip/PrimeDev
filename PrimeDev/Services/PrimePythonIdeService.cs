using System.Text;

namespace PrimeDev.Services
{
	public class PrimePythonIdeService
	{
		public HpAppDir CurrentApp { get; private set; }
		public string SelectedFile { get; private set; }

		private PrimeFileService primefiles;

		public PrimePythonIdeService(PrimeFileService file)
		{
			primefiles = file;
		}


		public void StartEditingPythonApp(string Appname)
		{
			CurrentApp = primefiles.PrimeData.Apps.Where(x => x.Name == Appname).FirstOrDefault();
			SelectedFile = CurrentApp.Files.FirstOrDefault().Key;
			_files = CurrentApp.Files.ToDictionary(x => x.Key, x => (false, Encoding.UTF8.GetString(x.Value)));

			NotifyAppSelected();
			NotifyFileSelected(SelectedFile);
		}

		public void SaveCurrentPythonApp()
		{

		}

		public void SelectFileToEdit(string filename)
		{
			SelectedFile = filename;
			NotifyFileSelected(filename);
		}

		public void FileContentHasChanged(string filename, string content)
		{
			_files[filename] = (true, content);
			NotifyFileEdited();
		}

		public void DeleteFile(string Filename)
		{
			_files.Remove(Filename);
			NotifyFileEdited();
		}

		public void AddNewFile(string Filename, string content = "")
		{
			if (this._files.ContainsKey(Filename))
			{
				Console.WriteLine("Sorry file already exists!");
			}
			else
			{
				_files.Add(Filename,(true,content));
			}
			NotifyFileEdited();
		}

		public async Task SaveAppToCalculator()
		{
			this.CurrentApp.Files = _files.ToDictionary(x => x.Key, x => Encoding.UTF8.GetBytes(x.Value.filecontent));
			await primefiles.UploadDataToPrime(this.CurrentApp);

			
			Console.WriteLine("HpApp written to Prime!");
		}


		public event Action<string> FileSelectedForEdit;
		private void NotifyFileSelected(string file) => FileSelectedForEdit?.Invoke(file);

		public event Action AppSelectedToEdit;
		private void NotifyAppSelected() => AppSelectedToEdit?.Invoke();

		public event Action FileHasBeenEdited;
		private void NotifyFileEdited() => FileHasBeenEdited?.Invoke();

		private Dictionary<string, (bool unsavededit, string filecontent)> _files;

		public Dictionary<string,(bool unsavededit, string filecontent)> AppFiles { get { return _files; } private set { _files = value; } }

	}
}
