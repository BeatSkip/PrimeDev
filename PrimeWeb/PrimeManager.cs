using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazm.Hid;

namespace PrimeWeb
{
	public class PrimeManager : IAsyncDisposable
	{
		private HidNavigator navigator;
		private List<HidDeviceFilter> filters;

		private PrimeCalculator CurrentPrime;

		private bool _initialized = false;


		public bool IsInitialized { get { return _initialized; } private set { _initialized = value; } }

		public PrimeManager(HidNavigator nav)
		{
			navigator = nav;
			System.Diagnostics.Debug.WriteLine("Started Prime manager!");
			filters = new List<HidDeviceFilter> { CreateHidFilter("0x03F0", "0x2441"), CreateHidFilter("0x03F0", "0x0441") };
			
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
				return true;
			}

			return false;
				
		}

		//public List<HidDevice> Calculators { get { return devices ?? new List<HidDevice>(); } }

		#region enumeration

		private static HidDeviceFilter CreateHidFilter(string VendorId, string ProductId)
		{
			int vid;
			int pid;

			if (VendorId.StartsWith("0x"))
				vid = Convert.ToInt32(VendorId, 16);
			else
				vid = int.Parse(VendorId, System.Globalization.NumberStyles.HexNumber);


			if (ProductId.StartsWith("0x"))
				pid = Convert.ToInt32(ProductId, 16);
			else
				pid = int.Parse(ProductId, System.Globalization.NumberStyles.HexNumber);

			return new HidDeviceFilter() { vendorId = vid, productId = pid };
		}

		#endregion

		#region Disposable

		public async ValueTask DisposeAsync()
		{
			IsInitialized = false;

				await navigator.DisposeAsync();
		}

		#endregion

	}
}
