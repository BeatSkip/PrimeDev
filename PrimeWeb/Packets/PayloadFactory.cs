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

	public static HpApp GenerateHpApp(FilePacketEventArgs e)
	{
		return new HpApp(e.Filename, e.PacketContent);
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
	}

	private void HandleFILE(byte[] payload)
	{
		logline($"received file, type: {((PrimeFileType)payload[6]).ToString()}!!");

		switch ((PrimeFileType)payload[6])
		{
			case PrimeFileType.APP:
				break;
			case PrimeFileType.APPNOTE:
				break;
			case PrimeFileType.APPPRGM:
				break;
			case PrimeFileType.COMPLEX:
				break;
			case PrimeFileType.LIST:
				break;
			case PrimeFileType.MATRIX:
				break;
			case PrimeFileType.NOTE:
				break;
			case PrimeFileType.PRGM:
				break;
			case PrimeFileType.REAL:
				break;
			case PrimeFileType.SETTINGS:		
				break;
			case PrimeFileType.TESTMODECONFIG:
				break;
			case PrimeFileType.UNKNOWN:
				
				break;
		}
	}

	private void HandleCHAT(byte[] payload)
	{

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
