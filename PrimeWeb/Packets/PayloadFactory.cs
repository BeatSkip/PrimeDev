namespace PrimeWeb.Packets;

public class PayloadFactory : IPayloadParser
{
	public bool IsRunningBackup { get; private set; } = false;

	private HpBackup backup;

	public PayloadFactory()
	{
		IsRunningBackup = false;
	}

	public void PublishNewPayload(byte[] data)
	{

	}

	public void StartBackup()
	{
		if (!IsRunningBackup)
		{
			backup = new HpBackup();
			logline("Starting new Calculator backup");
		}
		IsRunningBackup = true;
	}

	public void ParsePayload(byte[] payload)
	{
		PrimeCommand command = payload[0];

#if (DBG_PAYLOAD)
		logline($"Received payload from: {command.ToString()}");
#endif


		switch ((byte)command)
		{
			case PrimeCommands.INFOS:
				HandleINFOS(payload);
				break;
			case PrimeCommands.BACKUP:
				HandleBACKUP(payload);
				break;
			case PrimeCommands.FILE:
				HandleFILE(payload);
				break;
			case PrimeCommands.CHAT:
				HandleCHAT(payload);
				break;
			default:
				break;
		}

	}

	private void HandleINFOS(byte[] payload)
	{

	}

	private void HandleBACKUP(byte[] payload)
	{
		logline("End of Backup received");
		IsRunningBackup = false;
		OnBackupReceived(new BackupReceivedEventArgs() { Content = backup });

	}

	private void HandleFILE(byte[] payload)
	{

		bool iscompressed = (payload[8] == 0x00 && payload[9] == 0x00 && payload[10] == 0x78);

		byte[] data;

		if (iscompressed)
			data = HpFileParser.DecompressFileStream(payload.SubArray(10));
		else
			data = payload;


		logline($"received file, type: {((PrimeFileType)data[6]).ToString()}!!");
		//DbgTools.PrintPacket(data, maxlines: 5);
		(bool backup, PrimeFileType Type) handlestate = (IsRunningBackup, (PrimeFileType)data[6]);

		switch (handlestate)
		{

			case (true, PrimeFileType.APP): //App backup
				backup.Apps.Add(new HpApp(data));
				break;
			case (false, PrimeFileType.APP):
				break;
			case (true, PrimeFileType.APPNOTE)://appnote  backup
				break;
			case (false, PrimeFileType.APPNOTE):
				break;
			case (true, PrimeFileType.APPPRGM)://Appprogram backup
				break;
			case (false, PrimeFileType.APPPRGM):
				break;
			case (true, PrimeFileType.COMPLEX)://Complex backup
				break;
			case (false, PrimeFileType.COMPLEX):
				break;
			case (true, PrimeFileType.LIST)://List backup
				backup.Lists.Add(new HpList(data));
				break;
			case (false, PrimeFileType.LIST):
				break;
			case (true, PrimeFileType.MATRIX)://Matrix backup
				break;
			case (false, PrimeFileType.MATRIX):
				break;
			case (true, PrimeFileType.NOTE)://Note backup
				break;
			case (false, PrimeFileType.NOTE):
				break;
			case (true, PrimeFileType.PRGM)://Program backup
				break;
			case (false, PrimeFileType.PRGM):
				break;
			case (true, PrimeFileType.REAL)://REAL backup
				break;
			case (false, PrimeFileType.REAL):
				break;
			case (true, PrimeFileType.SETTINGS)://SETTINGS backup
				break;
			case (false, PrimeFileType.SETTINGS):
				break;
			case (true, PrimeFileType.TESTMODECONFIG)://TESTMODECONFIG backup
				break;
			case (false, PrimeFileType.TESTMODECONFIG):
				break;
			case (true, PrimeFileType.UNKNOWN)://unknown backup
				break;
			case (false, PrimeFileType.UNKNOWN):
				break;
		}

	}

	private void HandleCHAT(byte[] payload)
	{

	}

	/// <summary>
	/// Event to indicate Description
	/// </summary>
	public event EventHandler<BackupReceivedEventArgs> BackupReceived;
	/// <summary>
	/// Called to signal to subscribers that Description
	/// </summary>
	/// <param name="e"></param>
	protected virtual void OnBackupReceived(BackupReceivedEventArgs e)
	{
		var eh = BackupReceived;
		if (eh != null)
		{
			eh(this, e);
		}
	}



	/// <summary>
	/// Event to indicate Description
	/// </summary>
	public event EventHandler<AppReceivedEventArgs> AppReceived;
	protected virtual void OnAppReceived(AppReceivedEventArgs e)
	{
		var handler = AppReceived;
		if (handler != null) handler(this, e);
	}

	/// <summary>
	/// Event to indicate Description
	/// </summary>
	public event EventHandler<ChatReceivedEventArgs> ChatReceived;
	protected virtual void OnChatReceived(ChatReceivedEventArgs e)
	{
		var handler = ChatReceived;
		if (handler != null) handler(this, e);
	}

	/// <summary>
	/// Event to indicate Description
	/// </summary>
	public event EventHandler<FileReceivedEventArgs> FileReceived;
	protected virtual void OnFileReceived(ChatReceivedEventArgs e)
	{
		var handler = ChatReceived;
		if (handler != null) handler(this, e);
	}

	private void logline(string line)
	{
		Console.WriteLine($"[PayloadFactory] - {line}");
	}

}

public class AppReceivedEventArgs : EventArgs
{
	public string Name { get; set; }
	public HpApp App { get; set; }

}

public class ChatReceivedEventArgs : EventArgs
{
	public DateTime Date { get; set; }
	public string Message { get; set; }
}

public class FileReceivedEventArgs : EventArgs
{
	public string Data { get; set; }
}



public class BackupReceivedEventArgs : EventArgs
{
	public HpBackup Content { get; set; }
}

public class ChatEventArgs : EventArgs
{

}
