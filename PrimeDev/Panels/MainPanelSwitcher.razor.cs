using PrimeDev.Pages;

namespace PrimeDev.Panels;

public partial class MainPanelSwitcher
{

	[Inject]
	public PrimeManager manager { get; init; }
	[Inject]
	public MainPanelService panelservice { get; init; }

	[Inject]
	public PrimeFileService fileService { get; init; }

	protected override void OnInitialized()
	{
		currentpanelsetting = MainPanelContent.NotConnected;
		manager.OnChange += Manager_OnChange;
		panelservice.MainPanelRequested += Panelservice_MainPanelRequested;
	}

	private void Panelservice_MainPanelRequested(object? sender, PanelEventArgs e)
	{
		currentpanelsetting = e.content;
		StateHasChanged();
	}

	private void Manager_OnChange()
	{
		if (currentpanelsetting == MainPanelContent.NotConnected)
			this.currentpanelsetting = MainPanelContent.Empty;

		StateHasChanged();
	}

	private MainPanelContent currentpanelsetting { get; set; }






	public void Dispose()
	{
		
	}

	async void Prime_Connected(object? sender, EventArgs e)
	{

	}


	
}