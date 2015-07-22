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
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
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
            
            //Update Highscores
            foreach(var topic in topics)
            {
                int HighScore = 0;
                int Count = 0;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(topic))
                {               
                    HighScore = (int)(ApplicationData.Current.LocalSettings.Values[topic]);
                    Count = (int)(ApplicationData.Current.LocalSettings.Values[topic+"Count"]);
                }
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(topic + "Count"))
                {
                    Count = (int)(ApplicationData.Current.LocalSettings.Values[topic + "Count"]);
                }
                else
                {
                    //Count the words for the first time
                    string filename2 = "Data/" + topic + ".txt";
                    string fileContent2;
                    StorageFile file2 = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename2));
                    using (StreamReader sRead = new StreamReader(await file2.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
                        fileContent2 = await sRead.ReadToEndAsync();
                    Count = fileContent2.Split('\n').Length - 1;
                    ApplicationData.Current.LocalSettings.Values[topic + "Count"] = Count;
                }


                TextBlock txtbox = this.FindName(topic+"_1") as TextBlock;
                txtbox.Text = char.ToUpper(topic[0]) + topic.Substring(1) + " (" + HighScore + "/" + Count + ")";
            }
            
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = (e.ClickedItem as StackPanel).Name;
            Frame.Navigate(typeof(TestPageDetails), itemId);
        }
    }
}
