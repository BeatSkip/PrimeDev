
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace PrimeDev.Pages
{
    public partial class CodeEditor
    {
        
        [Inject]
        public PrimeManager manager { get; set; }

        [Inject]
        public PrimeFileService primefiles { get; set; }

        private PrimeCalculator prime;

        public HpApp CurrentApp { get; private set; }

        public string currentFile { get; set; }

		#region blazor main management

		protected override void OnInitialized()
        {
            prime = manager.GetCalculator();
            manager.OnChange += Manager_OnChange;
			primefiles.AppsChanged += Primefiles_AppsChanged;
            
        }

		private void Primefiles_AppsChanged(object? sender, EventArgs e)
		{
            StateHasChanged();
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

        private bool _open = false;
        public bool open { get { return _open; } set { _open = value; } } 

        void ToggleDrawer()
        {
            open = !open;


        }

        bool _openEnd = false;
        public bool openEnd { get { return _openEnd; } set { _openEnd = value; } }


        int drawerrightwidth = 0;

        void ToggleDrawerRight()
        {
            openEnd = !openEnd;

            if (openEnd)
			{
                drawerrightwidth = 350;
                filebrowsericon = Icons.Material.Filled.ArrowUpward;
			}
			else
			{
                drawerrightwidth = 0;
                filebrowsericon = Icons.Material.Filled.ArrowDownward;
            }
				

            StateHasChanged();

            _editor.Render();
        }

        string filebrowsericon = Icons.Material.Filled.ArrowDropDown;

        string getFileIcon(string filename)
		{
            if (filename.EndsWith(".png"))
                return Icons.Custom.FileFormats.FileImage;

            return Icons.Custom.FileFormats.FileDocument;
		}

        async Task DeleteSelectedFile()
		{

		}

        async Task CreateNewFile()
		{

		}

        #endregion

        #region Prime Code editor

        void SelectApplication(HpApp app)
        {
            Debug.WriteLine($"[IDE] - Swtiching to {app.Name}");

            int counter = 0;

            if (CurrentApp == null)
                this.openEnd = true;

            this.CurrentApp = app;
			StateHasChanged();

		}

		async void SelectFile(string Filename)
        {
            Debug.WriteLine($"[IDE] - Swtiching to file {Filename} from app: {this.CurrentApp.Name}");
            this.currentFile = Filename;
            this.ValueToSet = Encoding.UTF8.GetString(CurrentApp.Files[Filename]);
            await SetValue();
        }

        #endregion


        #region Monaco Editor
        private MonacoEditor _editor { get; set; }
        private string ValueToSet { get; set; }

        private StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor editor)
        {
            return new StandaloneEditorConstructionOptions
            {
                Language = "python",
                GlyphMargin = true,
                Value = "\"use strict\";\n" +
                        "function Person(age) {\n" +
                        "	if (age) {\n" +
                        "		this.age = age;\n" +
                        "	}\n" +
                        "}\n" +
                        "Person.prototype.getAge = function () {\n" +
                        "	return this.age;\n" +
                        "};\n",
             
            };
        }

        private async Task EditorOnDidInit(MonacoEditorBase editor)
        {
            await _editor.AddCommand((int)KeyMode.CtrlCmd | (int)KeyCode.KEY_H, (editor, keyCode) =>
            {
                Console.WriteLine("Ctrl+H : Initial editor command is triggered.");
            });

            var newDecorations = new ModelDeltaDecoration[]
            {
            new ModelDeltaDecoration
            {
                Range = new BlazorMonaco.Range(3,1,3,1),
                Options = new ModelDecorationOptions
                {
                    IsWholeLine = true,
                    ClassName = "decorationContentClass",
                    GlyphMarginClassName = "decorationGlyphMarginClass"
                }
            }
            };

            decorationIds = await _editor.DeltaDecorations(null, newDecorations);
            // You can now use 'decorationIds' to change or remove the decorations
        }

        private string[] decorationIds;

        private void OnContextMenu(EditorMouseEvent eventArg)
        {
            Console.WriteLine("OnContextMenu : " + System.Text.Json.JsonSerializer.Serialize(eventArg));
        }

        private async Task ChangeTheme(ChangeEventArgs e)
        {
            Console.WriteLine($"setting theme to: {e.Value.ToString()}");
            await MonacoEditor.SetTheme(e.Value.ToString());
        }

        private async Task SetValue()
        {
            Console.WriteLine($"setting value to: {ValueToSet}");
            await _editor.SetValue(ValueToSet);
        }

        private async Task GetValue()
        {
            var val = await _editor.GetValue();
            Console.WriteLine($"value is: {val}");
        }

        private async Task AddCommand()
        {
            await _editor.AddCommand((int)KeyMode.CtrlCmd | (int)KeyCode.Enter, (editor, keyCode) =>
            {
                Console.WriteLine("Ctrl+Enter : Editor command is triggered.");
            });
        }

        private async Task AddAction()
        {
            await _editor.AddAction("testAction", "Test Action", new int[] { (int)KeyMode.CtrlCmd | (int)KeyCode.KEY_D, (int)KeyMode.CtrlCmd | (int)KeyCode.KEY_B }, null, null, "navigation", 1.5, (editor, keyCodes) =>
            {
                Console.WriteLine("Ctrl+D : Editor action is triggered.");
            });
        }
        #endregion
    }
}
