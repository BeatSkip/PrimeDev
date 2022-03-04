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





		/// <summary>
		/// Event to indicate Description
		/// </summary>
		public event EventHandler<PanelEventArgs> MainPanelRequested;
		/// <summary>
		/// Called to signal to subscribers that Description
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMainPanelRequested(MainPanelContent e)
		{
			var handler = MainPanelRequested;
			if (handler != null) handler(this, new PanelEventArgs() { content = e });
		}


		public void SetMainPanel(MainPanelContent content)
		{
			OnMainPanelRequested(content);
			if (content == MainPanelContent.PythonIDE)
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
		PythonIDE,
		PPLIDE

	}

	public class PanelEventArgs : EventArgs
	{
		public MainPanelContent content { get; set; }
	}
}
