using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazm.Hid;
using PrimeWeb.Types;

namespace PrimeWeb
{
	public class PrimeManager : IAsyncDisposable
	{

		

		private HidNavigator navigator;
		private List<IHidFilter> filters;

		private PrimeCalculator CurrentPrime;

		private bool _initialized = false;


		public bool IsInitialized { get { return _initialized; } private set { _initialized = value; } }

		public PrimeManager(HidNavigator nav)
		{
			navigator = nav;
			System.Diagnostics.Debug.WriteLine("Started Prime manager!");
			filters = new List<IHidFilter> { new PrimeG2HidFilter(), new PrimeRevAHidFilter(), new PrimeRevCHidFilter() };
			
		}


		public PrimeCalculator GetCalculator()
		{
			return CurrentPrime;
		}

		public async Task<bool> InitializeAsync()
		{
			var filter = new HidDeviceRequestOptions() { filters = filters.ToArray() };
			var myDevice = await navigator.RequestDeviceAsync(filter);

			if (myDevice != null)
			{
				CurrentPrime = new PrimeCalculator(myDevice, myDevice.ProductName);
				IsInitialized = true;
				NotifyStateChanged();
				return true;
			}

			return false;
				
		}

		public void UpdateStatus(string text)
		{
			var prepend = CurrentPrime != null ? (CurrentPrime.IsConnected ? "Connected!" : "Disconected...") : "Idle..";
			Status = $"{prepend} | {text}";
			NotifyStatusUpdate();
		}

		public string Status { get; private set; }
        //public List<HidDevice> Calculators { get { return devices ?? new List<HidDevice>(); } }

        

		#region Disposable

		public async ValueTask DisposeAsync()
		{
			IsInitialized = false;

				await navigator.DisposeAsync();
		}

		#endregion

		#region event handling

		public event Action OnChange;
		private void NotifyStateChanged() => OnChange?.Invoke();

		public event Action OnStatusUpdate;
		private void NotifyStatusUpdate() => OnStatusUpdate?.Invoke();

		#endregion

	}
}
