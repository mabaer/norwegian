using System.Collections.ObjectModel;

namespace Vocabulary.ViewModels
{
    public class DesignViewModel
    {
        public ItemViewModel SelectedItem { get; set; }
        public ObservableCollection<ItemViewModel> Items { get; set; }
        public string Title { get; set; }
        public ObservableCollection<ItemViewModel> Columns { get; set; }
    }
}
