using PrimeWeb.HpTypes;

namespace PrimeDev.Shared
{
	public class MainPanelService
	{
		private PrimeFileService fileService;

		public MainPanelService()
		{

		}

		public void RegisterFileService(PrimeFileService service)
		{
			fileService = service;
		}

		public EventCallback<MainPanelContent> OnMainPanelRequested { get; set; }

		protected async void OnConfigureSettingsHandler(MainPanelContent panel)
		{
			await OnMainPanelRequested.InvokeAsync(panel);
		}

		public void SetMainPanel(MainPanelContent content)
		{
			OnConfigureSettingsHandler(content);
			if (content == MainPanelContent.IDE)
			{
				OnIdeModeChange(true);
				ide_enabled = true;
			}
			else
			{
				if (ide_enabled)
				{
					OnIdeModeChange(false);
				}
			}
				

		}

		private bool ide_enabled = false;

		public EventCallback<bool> GlobalIDEModeChanged { get; set; }

		protected async void OnIdeModeChange(bool enabled)
		{
			await GlobalIDEModeChanged.InvokeAsync(enabled);
		}

	}

	public enum MainPanelContent
	{
		NotConnected,
		Empty,
		CalcSettings,
		RawDataViewer,
		IDE

	}
}
