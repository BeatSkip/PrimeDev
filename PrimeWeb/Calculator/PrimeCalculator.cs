using Blazm.Hid;


namespace PrimeWeb.Calculator;

/// <summary>
/// Represents an HP Prime
/// </summary>
public class PrimeCalculator
{

	public string ProductName { get { return DeviceInfo.Product; } }

	private IHidDevice prime;
	private FrameWorker frameWorker;
	private PayloadFactory dataFactory;

	public PrimeCalculator(IHidDevice device, string Title = "")
	{
		prime = device;
		frameWorker = new FrameWorker(device);
		frameWorker.CommunicationInitialized += CommunicationInitialized;
		dataFactory = new PayloadFactory(frameWorker);
		dataFactory.OnCalculatorInfoReceived += CalcInfoReceived;
		dataFactory.BackupReceived += DataFactory_BackupReceived;
	}

	private void DataFactory_BackupReceived(object? sender, BackupEventArgs e)
	{
		OnBackup(e.Content);
	}

	private void CalcInfoReceived(HpInfos info)
	{
		this.DeviceInfo = info;
		OnChanged();
	}
	private void CommunicationInitialized()
	{
		OnConnected(); 
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

		await frameWorker.ConnectAsync();
		//await packer.InitializeAsync();

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
	

	public async Task SendRequest(PrimeCommand command)
	{
		if (command == PrimeCommands.BACKUP)
		{
			frameWorker.StartBackup();
			dataFactory.StartBackup();
		}
			

		
		await frameWorker.Send(new PrimeRequest(command));
	}

	public async Task SendChatMessage(string Message)
	{
		if (!IsConnected) return;
		var payload = ChatConverter.CreateChat(Message);
		await frameWorker.Send(payload);
	}

	public async Task SendHpApp(HpAppDir app)
	{
		await frameWorker.Send(app);
	}

	#endregion

	#region eventhandlers

	/// <summary>
	/// Reports physical device events
	/// </summary>
	

	/// <summary>
	/// Reports data received from the USB
	/// </summary>
	public event EventHandler<DataReceivedEventArgs> DataReceived;

	/// <summary>
	/// Reports message from the USB
	/// </summary>


	#endregion

	#region reception events


	public event EventHandler<BackupEventArgs> BackupReceived;
	protected virtual void OnBackup(HpBackup backup)
	{
		var handler = BackupReceived;
		if (handler != null) handler(this, new BackupEventArgs() { Content = backup});
	}
	#endregion


	#region events

	public event EventHandler<EventArgs> Connected, Disconnected, Changed;

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

}

