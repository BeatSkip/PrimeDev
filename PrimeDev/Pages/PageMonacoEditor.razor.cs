using BlazorPro.BlazorSize;
using PrimeDev.Services;

namespace PrimeDev.Pages
{
	public partial class PageMonacoEditor
	{
		string currentfilename = "";
		protected override void OnInitialized()
		{
			base.OnInitialized();



		}

		private async Task OnContentSizeChanged(ModelContentChangedEvent data)
		{
			editcounter++;
			if (editcounter > 1)
			{
				var val = await _editor.GetValue();
				Console.WriteLine("File has been edited!");
				pyserverice.FileContentHasChanged(this.pyserverice.SelectedFile, val);
				editcounter = 0;
			}

		}

		private async void Pyserverice_FileSelectedForEdit(string obj)
		{
			editcounter = 0;
			currentfilename = obj;
			await this.SetValue(pyserverice.AppFiles[obj].filecontent);

		}


		protected override void OnAfterRender(bool firstRender)
		{

			if (firstRender)
			{
				// Subscribe to the OnResized event. This will do work when the browser is resized.
				listener.OnResized += WindowResized;
			}
		}

		async void WindowResized(object _, BrowserWindowSize window)
		{
			
			await _editor.Layout();
			Console.WriteLine("Window Resized!!!");
		}

	}


}
