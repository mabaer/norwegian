using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Vocabulary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PronouncePage : Page
    {
        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }
        public PronouncePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //Read Data Vowels
            //Vowels
            string filename = "Data/Vowels.txt";
            string fileContent;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
            fileContent = await sRead.ReadToEndAsync();
            TextBlock tb = FindChildControl<TextBlock>(vowelSection, "textbox1") as TextBlock;
            tb.Text = fileContent;

            //Consonants
            filename = "Data/Consonants.txt";
            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
            fileContent = await sRead.ReadToEndAsync();
            tb = FindChildControl<TextBlock>(consonantsSection, "textbox1") as TextBlock;
            tb.Text = fileContent;

            //Diphthongs
            filename = "Data/Diphthongs.txt";
            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
            fileContent = await sRead.ReadToEndAsync();
            tb = FindChildControl<TextBlock>(diphSection, "textbox1") as TextBlock;
            tb.Text = fileContent;

            //NOT LOADED BECAUSE OF ERROR WITH 3rd HUBSECTION
            //Exceptions
            //filename = "Data/Exceptions.txt";
            //file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename));
            //using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
            //fileContent = await sRead.ReadToEndAsync();
            //tb = FindChildControl<TextBlock>(exceptionsSection, "textbox1") as TextBlock;
            //tb.Text = fileContent;

            
        }
    }
}
