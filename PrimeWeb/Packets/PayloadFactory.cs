using PrimeWeb.Files;

namespace PrimeWeb.Packets;

//TODO: Phase out Payloadfactory
public partial class PayloadFactory : IPayloadParser
{
	public bool IsRunningBackup { get; private set; } = false;

	private HpCalcContents backup;
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
			backup = new HpCalcContents();
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
		backup.AddFile(payload);

		bool iscompressed = (payload[8] == 0x00 && payload[9] == 0x00 && payload[10] == 0x78);

		byte[] data;

		if (iscompressed)
		{
			data = HpFileParser.DecompressFileStream(payload.SubArray(10));
			//DbgTools.PrintPacket(data, title: "raw uncompressed dump");
		}
		else
			data = payload;

		
		byte handlestate = (PrimeDataType)data[6];
		logline($"received file, type: {((PrimeDataType)data[6]).ToString()}!!");
		//DbgTools.PrintPacket(data, maxlines: 5);
		

		

		if (handlestate == Calculator.PrimeDataTypes.SETTINGS)//preprocess settings
			handleSettings(data);

		switch (handlestate)
		{

			case PrimeDataTypes.APP: //App backup
				backup.Apps.Add(new HpAppDir(data));
				break;
			case PrimeDataTypes.APPNOTE://appnote  backup
				break;
			case PrimeDataTypes.APPPRGM://Appprogram backup

				break;
			case PrimeDataTypes.COMPLEX:
				break;
			case PrimeDataTypes.LIST://List backup
				handleLIST(data);
				//DbgTools.PrintPacket(data, title: "LIST");
				//backup.Lists.Add(new HpList(data));
				break;
			case PrimeDataTypes.MATRIX://Matrix backup
				break;
			case PrimeDataTypes.NOTE://Note backup
				break;
			case PrimeDataTypes.PRGM://Program backup
				backup.Programs.Add(new HpProgram(data));
				break;
			case PrimeDataTypes.REAL://REAL backup
				var real = HP_Real.FromBytes(data);

				break;
			case PrimeDataTypes.SETTINGS://SETTINGS backup
				backup.CALCSettings = new HpCalcSettings(data);
				break;
			case PrimeDataTypes.TESTMODECONFIG://TESTMODECONFIG backup
				break;
			case PrimeDataTypes.UNKNOWN://unknown backup
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


