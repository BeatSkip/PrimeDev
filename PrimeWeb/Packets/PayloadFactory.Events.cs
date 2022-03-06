using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Packets
{
	public partial class PayloadFactory
	{


		public Action<HpInfos> OnCalculatorInfoReceived { get; set; }
		private void NotifyCalculatorInfoReceived(HpInfos data) => OnCalculatorInfoReceived?.Invoke(data);


		/// <summary>
		/// Event that triggers when a full backup has been received
		/// </summary>
		public event EventHandler<BackupEventArgs> BackupReceived;
		protected virtual void OnBackupReceived(BackupEventArgs e)
		{
			var eh = BackupReceived;
			if (eh != null)
			{
				eh(this, e);
			}
		}

		/// <summary>
		/// Event that triggers when an app has been received
		/// </summary>
		public event EventHandler<AppEventArgs> AppReceived;
		protected virtual void OnAppReceived(AppEventArgs e)
		{
			var handler = AppReceived;
			if (handler != null) handler(this, e);
		}

		/// <summary>
		/// Event that triggers when a chat message has been received
		/// </summary>
		public event EventHandler<ChatEventArgs> ChatReceived;
		protected virtual void OnChatReceived(ChatEventArgs e)
		{
			var handler = ChatReceived;
			if (handler != null) handler(this, e);
		}

		/// <summary>
		/// Event that triggers when a file has been received
		/// </summary>
		public event EventHandler<FileEventArgs> FileReceived;
		protected virtual void OnFileReceived(FileEventArgs e)
		{
			var handler = FileReceived;
			if (handler != null) handler(this, e);
		}

	}
}
