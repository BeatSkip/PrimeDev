using PrimeWeb.Files;

namespace PrimeWeb.Packets;

public partial class PayloadFactory : IPayloadParser
{
	public bool IsRunningBackup { get; private set; } = false;

	private HpBackup backup;
	private FrameWorker worker;

	public PayloadFactory(FrameWorker frameworker)
	{
		worker = frameworker;
		worker.CalcInfoReceived += CalcInfoReceived;
		worker.MessageReceived += ParsePayload;
	}

	private void CalcInfoReceived(HpInfos info)
	{
		NotifyCalculatorInfoReceived(info);
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
		OnBackupReceived(new BackupEventArgs() { Content = backup });

	}

	private void HandleFILE(byte[] payload)
	{

		bool iscompressed = (payload[8] == 0x00 && payload[9] == 0x00 && payload[10] == 0x78);

		byte[] data;

		if (iscompressed)
		{
			data = HpFileParser.DecompressFileStream(payload.SubArray(10));
			//DbgTools.PrintPacket(data, title: "raw uncompressed dump");
		}
		else
			data = payload;


		logline($"received file, type: {((PrimeFileType)data[6]).ToString()}!!");
		//DbgTools.PrintPacket(data, maxlines: 5);
		(bool backup, PrimeFileType Type) handlestate = (IsRunningBackup, (PrimeFileType)data[6]);

		if (handlestate.Type == PrimeFileType.SETTINGS)//preprocess settings
			handleSettings(data);

		switch (handlestate)
		{

			case (true, PrimeFileType.APP): //App backup
				backup.Apps.Add(new HpAppDir(data));
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
				handleLIST(data);
				DbgTools.PrintPacket(data, title: "LIST");
				//backup.Lists.Add(new HpList(data));
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
				backup.Programs.Add(new HpProgram(data));
				break;
			case (false, PrimeFileType.PRGM):
				break;
			case (true, PrimeFileType.REAL)://REAL backup
				break;
			case (false, PrimeFileType.REAL):
				break;
			case (true, PrimeFileType.SETTINGS)://SETTINGS backup
				backup.CALCSettings = new HpCalcSettings(data);
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

	private void handleSettings(byte[] data, bool backup = true)
	{
		var info = HpSettingsBuilder.GetFileName(data);

		Console.WriteLine($"[SETTINGS] - filename: {info.name}");


		switch (info.name)
		{
			case "calc.hpsettings":
				var parsedcalc = new HpCalcSettings(data);
				break;
			case "cas.hpsettings":
				var parsedcas = new HpCasSettings(data);
				break;
			case "settings":
				//TODO: parse general settings
				break;
			case "calc.hpvars":
				break;

		}
	}

	private void HandleCHAT(byte[] payload)
	{

	}


	private void handleLIST(byte[] payload)
	{
		PrimeFileParser.Parse_List(payload);
	}

	#region Events




	#endregion


	private void logline(string line)
	{
		Console.WriteLine($"[PayloadFactory] - {line}");
	}

}


