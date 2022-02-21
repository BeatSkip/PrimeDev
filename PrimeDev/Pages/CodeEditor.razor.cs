
using Microsoft.AspNetCore.Components.Web;

namespace PrimeDev.Pages
{
    public partial class CodeEditor
    {

        [Inject]
        public PrimeManager manager { get; set; }

        [Inject]
        public PrimeFileService primefiles { get; set; }

        private PrimeCalculator prime;

        protected override void OnInitialized()
        {
            prime = manager.GetCalculator();
            manager.OnChange += Manager_OnChange;
			primefiles.AppsChanged += Primefiles_AppsChanged; ;
        }

		private void Primefiles_AppsChanged(object? sender, EventArgs e)
		{
			
		}

		private void Manager_OnChange()
		{
            StateHasChanged();
		}

		public void Dispose()
        {
            manager.OnChange -= Manager_OnChange;
            primefiles.AppsChanged -= Primefiles_AppsChanged;
        }
    }
}
