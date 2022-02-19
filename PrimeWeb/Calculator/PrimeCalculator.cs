using System;
using Blazm.Hid;
using PrimeWeb.Utility;
using PrimeWeb.Calculator;
using PrimeWeb.Packets;
using PrimeWeb.Types;
using PrimeWeb.Protocol;
using System.Drawing;

namespace PrimeWeb
{
    /// <summary>
    /// Represents an HP Prime
    /// </summary>
    public class PrimeCalculator
    {
        private Action<string> screenshotCallback; 

        public string ProductName { get { return DeviceInfo.Product; } }

        private IHidDevice prime;

        private FrameWorker packetWorker;

        public PrimeCalculator(IHidDevice device, string Title = "")
        {
            prime = device;
            packetWorker = new FrameWorker(device);
			packetWorker.CalcInitialized += PacketWorker_CalcInitialized;
        }

		private void PacketWorker_CalcInitialized(object? sender, CommsInitEventArgs e)
		{
            this.DeviceInfo = e.Info;
            this.InfoChanged();
            ConnectionInitDone();
        }

		private void PacketWorker_V2MessageReceived(byte[] data)
		{
			
            var header = data.SubArray(0, 64);
            DbgTools.PrintPacket(header);

			switch ((PrimeCMD)data[0])
			{
                case (PrimeCMD.RECV_SCREEN):
                    Console.WriteLine("Received Screenshot!!");
                    ProcessScreenshot(data);
                    break;
                case (PrimeCMD.RECV_FILE):
                    Console.WriteLine("Received File!!");
                    ProcessFileInput(data);
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

            await prime.CloseAsync();

            OnDisconnected();

        }
        #endregion

        #region General methods

        public async Task RequestBackup()
        {
            if (!IsConnected) return;

            var payload = new byte[1] { (byte)PrimeCMD.RECV_BACKUP};

            await packetWorker.SendPayload(payload);
        }

        public async Task RequestSettings()
		{
            if (!IsConnected) return;

            var payload = PrimeCommander.GetPayloadRequestSettings();

            await packetWorker.SendPayload(payload);
        }

        public async Task SendKey(uint key)
		{
            if (!IsConnected) return;

            var payload = PrimeCommander.GetPayloadSendKey(key);

            await prime.SendReportAsync(0x00,payload);
        }

        public async Task SendChatMessage(string Message)
        {
            if (!IsConnected) return;

            var payload = PrimeCommander.GetPayloadMessage(Message);

            await packetWorker.SendPayload(payload);


        }

        public async Task GetScreenshot(Action<string> callback)
		{
            if (!IsConnected) return;

            screenshotCallback = callback;
            var payoad = new byte[] { (byte)PrimeCMD.RECV_SCREEN, 0x08, 0x00, 0x00, 0x00, 0x01, 0x03 , 0x00};
            await packetWorker.SendPayload(payoad);
        }

		#endregion

		#region Data Processing

		private void ProcessScreenshot(byte[] data)
		{
            var img = ParseCommandScreenshot(data.SubArray(13));

            screenshotCallback.Invoke(img);
            
        }

        private void ProcessFileInput(byte[] data)
		{
			Console.WriteLine("Received File Input!");
            byte[] result;
            if(data[10] == 0x78)
			{
                result = PrimeCommander.decompress(data.SubArray(10));
			}
			else
			{
                result = data.SubArray(10);
			}

            DbgTools.PrintPacket(result);
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
        

        #endregion


        #region events

        internal void ConnectionInitDone()
		{
            OnConnected();
        }

        internal void InfoChanged()
		{
            OnChanged();
		}

        internal void TransferCompleted(byte[] data)
        {
            OnTransferCompleted(new DataReceivedEventArgs(data));
        }

        /// <summary>
        /// Some data arrived (Device has to be Receiving data)
        /// </summary>
        /// <param name="e">Data received</param>
        protected virtual void OnTransferCompleted(DataReceivedEventArgs e)
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

        #region LegacyCode
        /*
         /// <summary>
        /// Chat message has been received!
        /// </summary>
        /// <param name="e">Chat message received</param>
        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            var handler = ChatMessageReceived;
            if (handler != null) handler(this, e);
        }
         */

        #endregion
    }

}
