using PrimeDev.Pages;

namespace PrimeDev.Panels;

public partial class MainPanelSwitcher
{

	[Inject]
	public PrimeManager manager { get; init; }

	[Inject]
	public PrimeFileService fileService { get; init; }

	protected override void OnInitialized()
	{
		currentpanelsetting = MainPanelContent.NotConnected;
		manager.OnChange += Manager_OnChange;
	}

	private void Manager_OnChange()
	{
		if (currentpanelsetting == MainPanelContent.NotConnected)
			this.currentpanelsetting = MainPanelContent.Empty;

		StateHasChanged();
	}

	private MainPanelContent currentpanelsetting { get; set; }


	private void MainpanelRequested(MainPanelContent content)
	{

	}



	public void Dispose()
	{
		
	}

	async void Prime_Connected(object? sender, EventArgs e)
	{

	}


	
}