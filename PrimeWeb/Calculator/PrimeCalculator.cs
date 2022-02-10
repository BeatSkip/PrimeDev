﻿using System;
using Blazm.Hid;

namespace PrimeWeb
{
    /// <summary>
    /// Represents an HP Prime
    /// </summary>
    public class PrimeCalculator
    {
        public string ProductName { get { return prime.ProductName; } }


        private HidDevice prime;

        private bool _isConnected,_continue;

        /// <summary>
        /// Reports physical device events
        /// </summary>
        public event EventHandler<EventArgs> Connected, Disconnected;
        /// <summary>
        /// Reports data received from the USB
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataReceived;



        public PrimeCalculator(HidDevice device, string Title = "")
		{
            prime = device;

        }


        /// <summary>
        /// Checks the Hid Devices looking for the first calculator
        /// </summary>
        public async Task Connect()
        { 
            await prime.OpenAsync();
            if (prime.Opened)
            {
                OnConnected();
                prime.Notification += Prime_Notification;
            }
                

        }

        private void Prime_Notification(object? sender, OnInputReportArgs e)
        {
            Console.WriteLine("Received Notification!");
            OnReport(e);
        }

        /// <summary>
        /// There is at least one compatible device connected
        /// </summary>
        public bool IsConnected
        {
            get { return prime.Opened; }
        }

        /// <summary>
        /// First compatible device was found and it is connected
        /// </summary>
        protected virtual void OnConnected()
        {
            var handler = Connected;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// The last connected device was removed
        /// </summary>
        protected virtual void OnDisconnected()
        {
            var handler = Disconnected;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sends data to the calculator
        /// </summary>
        /// <param name="file">Data to send</param>
        public async Task Send(PrimeUsbData file)
        {
            if (!IsConnected) return;

            Console.WriteLine("Sending chunks!");
            foreach(var c in file.Chunks)
            {
                await prime.SendReportAsync(0, c);
            }
                
        }

        private bool IsNotReady()
        {
            return !(_isConnected && prime != null);
        }

        /// <summary>
        /// Some data arrived (Device has to be Receiving data)
        /// </summary>
        /// <param name="e">Data received</param>
        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            
            var handler = DataReceived;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Size of the output chunk (Output Report lenght)
        /// </summary>
        public int OutputChunkSize
        {
            //get { return prime.Capabilities.OutputReportByteLength; }
            get { return 128; }
        }

        /// <summary>
        /// Enabled the data reception for this device, flushing any pending data in the buffer
        /// </summary>
        public void StartReceiving()
        {
            // Flush contents
            StopReceiving();

            _continue = true;

           // if (prime != null)
             //   prime.ReadReport(OnReport);
        }

        /// <summary>
        /// Disables the data reception
        /// </summary>
        public async void StopReceiving()
        {
            _continue = false;

            if (prime != null)
                await prime.CloseAsync();
        }

        private void OnReport(OnInputReportArgs report)
        {
           // report;
           // i//f(_continue)
               // prime.ReadReport(OnReport); // Expect more reports

            if (IsNotReady()) return;

            OnDataReceived(new DataReceivedEventArgs(report.Data));
        }


		#region Enumeration and HID

       
		#endregion
	}
}
