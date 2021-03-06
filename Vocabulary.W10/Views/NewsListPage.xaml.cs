using Windows.UI.Xaml.Navigation;
using AppStudio.Common;
using AppStudio.DataProviders.Rss;
using Vocabulary;
using Vocabulary.Sections;
using Vocabulary.ViewModels;

namespace Vocabulary.Views
{
    public sealed partial class NewsListPage : PageBase
    {
        public NewsListPage()
        {
            this.ViewModel = new ListViewModel<RssSchema>(new NewsConfig());
            this.InitializeComponent();
        }

        public ListViewModel<RssSchema> ViewModel { get; set; }

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
