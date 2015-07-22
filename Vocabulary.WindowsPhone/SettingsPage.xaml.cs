using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void resetHighscores_Click(object sender, RoutedEventArgs e)
        {
            var messgeDialog = new MessageDialog("Do you really want to reset all highscores?");
            messgeDialog.Commands.Add(new UICommand("Yes"));
            messgeDialog.Commands.Add(new UICommand("No"));
            messgeDialog.DefaultCommandIndex = 0;
            messgeDialog.CancelCommandIndex = 1;
            var result = await messgeDialog.ShowAsync();
            if (result.Label.Equals("Yes"))
            {
                resetHighscore();
            }
        }

        private async void resetHighscore()
        {
            //Get Topics
            string filename = "Data/topics.txt";
            string fileContent;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
                fileContent = await sRead.ReadToEndAsync();
            //Read Data
            int topicCount = fileContent.Split('\n').Length;
            string[] topics = new string[topicCount];
            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    topics[i] = line;
                    i++;
                }
            }
            
            //Iterate over all highscores
            foreach (var topic in topics)
            {
                ApplicationData.Current.LocalSettings.Values[topic] = 0;
            }
        }
    }
}
