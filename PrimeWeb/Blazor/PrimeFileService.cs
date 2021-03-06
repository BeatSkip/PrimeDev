using PrimeWeb.Types;

namespace PrimeWeb.Blazor
{
	//TODO: phase out PrimeFileService
	public class PrimeFileService
	{
		public string selectedApp { get; set; }

		private PrimeCalculator prime;

		public HpCalcContents PrimeData { get; set; }

		public bool HasBackup { get { return PrimeData != null; } }

		public PrimeFileService()
		{

		}

		public void RegisterCalculator(PrimeCalculator calc)
		{
			this.prime = calc;
			this.prime.BackupReceived += Prime_BackupReceived;

		}

		private void Prime_BackupReceived(object? sender, BackupEventArgs e)
		{
			this.PrimeData = e.Content;
			this.OnAppsChanged();
		}


		public async Task UploadDataToPrime(HpAppDir app)
		{
			await prime.SendHpApp(app);
		}

		/// <summary>
		/// Event to indicate Description
		/// </summary>
		public event EventHandler AppsChanged;
		/// <summary>
		/// Called to signal to subscribers that Description
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnAppsChanged()
		{
			EventHandler eh = AppsChanged;
			if (eh != null)
			{
				eh(this, EventArgs.Empty);
			}
		}

	}
}
