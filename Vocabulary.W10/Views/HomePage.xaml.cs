using Windows.UI.Xaml.Navigation;
using Vocabulary;
using Vocabulary.ViewModels;

namespace Vocabulary.Views
{
    public sealed partial class HomePage : PageBase
    {
        public HomePage()
        {
            this.ViewModel = new MainViewModel(8);            
            this.InitializeComponent();
        }

        public MainViewModel ViewModel { get; set; }

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }
    }
}
