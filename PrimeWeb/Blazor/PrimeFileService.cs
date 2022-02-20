using PrimeWeb.Types;

namespace PrimeWeb.Blazor
{
	public class PrimeFileService
	{

		private PrimeCalculator prime;


		public Dictionary<string, HpApp> Applist { get; private set; } = new Dictionary<string, HpApp>();

		public PrimeFileService()
		{

		}

		public void RegisterCalculator(PrimeCalculator calc)
		{
			this.prime = calc;
			this.prime.AppReceived += Prime_AppReceived;

		}

		private void Prime_AppReceived(object? sender, AppReceivedEventArgs e)
		{
			var app = e.App;

			if (Applist.ContainsKey(app.Name))
			{
				Console.WriteLine("[PrimeFileService] - Received existing app! dumping new...");
			}
			else
			{
				Console.WriteLine("Added app to list!");
				Applist.Add(app.Name, app);
				this.OnAppsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Event to indicate Description
		/// </summary>
		public event EventHandler AppsChanged;
		/// <summary>
		/// Called to signal to subscribers that Description
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnAppsChanged(EventArgs e)
		{
			EventHandler eh = AppsChanged;
			if (eh != null)
			{
				eh(this, e);
			}
		}

	}
}
