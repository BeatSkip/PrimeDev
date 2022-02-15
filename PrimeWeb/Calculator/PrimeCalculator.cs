using System;
using Blazm.Hid;
using PrimeWeb.Utility;
using PrimeWeb.Calculator;
using PrimeWeb.Packets;
using PrimeWeb.HpTypes;
using System.Drawing;

namespace PrimeWeb
{
    /// <summary>
    /// Represents an HP Prime
    /// </summary>
    public class PrimeCalculator
    {
        private Action<string> screenshotCallback; 

        public string ProductName { get { return prime.ProductName; } }

        private HidDevice prime;

        private PacketWorker packetWorker;

        public PrimeCalculator(HidDevice device, string Title = "")
        {
            prime = device;
            packetWorker = new PacketWorker(this,device);
			packetWorker.V2MessageReceived += PacketWorker_V2MessageReceived;
        }

		private void PacketWorker_V2MessageReceived(object? sender, V2MessageEventArgs e)
		{
			
            var header = e.Data.SubArray(0, 64);
            DbgTools.PrintPacket(header);

			switch ((PrimeCMD)e.Data[0])
			{
                case (PrimeCMD.RECV_SCREEN):
                    Console.WriteLine("Received Screenshot!!");
                    ProcessScreenshot(e.Data);
                    break;
            }
		}

		#region properties

		/// <summary>
		/// Device information like software version and Serial number
		/// </summary>
		public HpInfos DeviceInfo { get; internal set; }

        /// <summary>
        /// There is at least one compatible device connected
        /// </summary>
        public bool IsConnected
        {
            get { return prime.Opened; }
        }


        /// <summary>
        /// Size of the output chunk (Output Report lenght)
        /// </summary>
        public int OutputChunkSize
        {
            //get { return prime.Capabilities.OutputReportByteLength; }
            get { return 1024; }
        }

		#endregion

		#region Connection methods

		/// <summary>
		/// Checks the Hid Devices looking for the first calculator
		/// </summary>
		public async Task Initialize()
        {
            if (prime == null)
                return;

            await packetWorker.ConnectAsync();


        }

        public async Task Disconnect()
		{
            if (prime == null)
                return;

            if (!prime.Opened)
                return;

            //prime.Notification -= Prime_Notification;

            await prime.CloseAsync();

            OnDisconnected();

        }
		#endregion

		#region General methods

		/// <summary>
		/// Sends data to the calculator
		/// </summary>
		/// <param name="file">Data to send</param>
		internal async Task Send(PrimeUsbData file)
        {
            if (!IsConnected) return;

        }

        public async Task SendChatMessage(string Message)
        {
            if (!IsConnected) return;

            var data = new PrimeUsbData(Message, 1024, null);

            //await this.Send(data);

        }

        public async Task GetScreenshot(Action<string> callback)
		{
            if (!IsConnected) return;

            screenshotCallback = callback;
            var pkt_prot = MessageUtils.Misc.GetPacketSetProtocolV2();
            await prime.SendReportAsync(pkt_prot.id, pkt_prot.data);

            var packet = MessageUtils.Misc.GetPacketRequestScreen(ScreenFormat.PNG_320px_240px_16bit);
            await prime.SendReportAsync(packet.id, packet.data);
        }

        private void ProcessScreenshot(byte[] data)
		{
            var img = ParseCommandScreenshot(data.SubArray(13));

            screenshotCallback.Invoke(img);
            
        }

        private string ParseCommandScreenshot(byte[] data)
		{
            return Convert.ToBase64String(data);
        }

        #endregion

        #region eventhandlers

        /// <summary>
        /// Reports physical device events
        /// </summary>
        public event EventHandler<EventArgs> Connected, Disconnected, Changed;

        /// <summary>
        /// Reports data received from the USB
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Reports message from the USB
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> ChatMessageReceived;

        #endregion


        #region events


        internal void InfoChanged()
		{
            OnChanged();
		}

        private void OnReport(OnInputReportArgs report)
        {
            OnDataReceived(new DataReceivedEventArgs(report.Data));
        }

        private void OnMessage(PrimeChunk chunk)
        {
            var args = new MessageReceivedEventArgs() { Message = chunk.ToString(), Source = chunk };

            OnMessageReceived(args);
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
        /// Some data arrived (Device has to be Receiving data)
        /// </summary>
        /// <param name="e">Data received</param>
        protected virtual void OnChanged()
        {
            var handler = Changed;
            if (handler != null) handler(this, new EventArgs());
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

        #endregion

        #region Enumeration and HID


        #endregion
    }

	public class MessageReceivedEventArgs : EventArgs{
        public string Message { get; set; }
        public PrimeChunk Source { get; set; }
    }
}
