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

		
	}
}
