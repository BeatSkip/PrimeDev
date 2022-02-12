using System;
using Blazm.Hid;
using PrimeWeb.Utility;
using PrimeWeb.Calculator;

namespace PrimeWeb
{
    /// <summary>
    /// Represents an HP Prime
    /// </summary>
    public class PrimeCalculator
    {
        public string ProductName { get { return prime.ProductName; } }


        private HidDevice prime;

        private bool _isConnected, _continue;

        /// <summary>
        /// Reports physical device events
        /// </summary>
        public event EventHandler<EventArgs> Connected, Disconnected;

        /// <summary>
        /// Reports data received from the USB
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Reports message from the USB
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> ChatMessageReceived;


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
            var rcv = new PrimeChunk(e.ReportId, e.Data);
            rcv.Print();

            DbgTools.PrintPacket(e.Data);
			/*

            List<byte> alldat = new List<byte>();
            alldat.Add((byte)e.ReportId);
            alldat.AddRange(e.Data);

            var data = new PrimeUsbData(alldat.ToArray());
            Console.WriteLine($"\nData valid: {data.IsValid}");
            Console.WriteLine($"Data complete: {data.IsComplete}");
            Console.WriteLine($"Data name: {data.Name}");
            Console.WriteLine($"Data name: {BitConverter.ToString(data.Data)}");
            */
			switch (rcv.Type)
			{
                case(PacketType.Chat):
                    OnMessage(rcv);
                    break;
                default:
                    OnReport(e);
                    break;
            }
            
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
            foreach (var c in file.Chunks)
            {
                byte id = c.ReportID;
                byte[] data = c.Data;
                await prime.SendReportAsync(id, data);
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
        /// Chat message has been received!
        /// </summary>
        /// <param name="e">Chat message received</param>
        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            var handler = ChatMessageReceived;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Size of the output chunk (Output Report lenght)
        /// </summary>
        public int OutputChunkSize
        {
            //get { return prime.Capabilities.OutputReportByteLength; }
            get { return 1024; }
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
            //report;
            //if(_continue)
            //  prime.ReadReport(OnReport); // Expect more reports

            if (IsNotReady()) return;

            OnDataReceived(new DataReceivedEventArgs(report.Data));
        }

        private void OnMessage(PrimeChunk chunk)
        {
            var args = new MessageReceivedEventArgs() { Message = chunk.ToString(), Source = chunk };

            OnMessageReceived(args);
        }


        #region Enumeration and HID


        #endregion
    }

    public class MessageReceivedEventArgs : EventArgs{
        public string Message { get; set; }
        public PrimeChunk Source { get; set; }
    }
}
