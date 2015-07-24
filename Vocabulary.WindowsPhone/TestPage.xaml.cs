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
/*
 *  Copyright 2015 Marc-André Bär
 
 *  This project is for educational use only.
 
 *  This file is part of Norsk_Vocabulary.
    Norsk_Vocabulary is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    Norsk_Vocabulary is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with Norsk_Vocabulary If not, see <http://www.gnu.org/licenses/>.
  
 */
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
            //Get Categories
            string filename = "Data/topics.txt";
            string fileContent;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
                fileContent = await sRead.ReadToEndAsync();
            //Read Data
            int topicCount = fileContent.Split('\n').Length;
            string[] topics = new string[topicCount];
            //Iterate over the lines and add the categories to the array
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
                //Init values
                int HighScore = 0;
                int Count = 0;
                //If there is already a highscore load highscore and count
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(topic))
                {               
                    HighScore = (int)(ApplicationData.Current.LocalSettings.Values[topic]);
                    Count = (int)(ApplicationData.Current.LocalSettings.Values[topic+"Count"]);
                }
                //If there is just a count load it
                else if (ApplicationData.Current.LocalSettings.Values.ContainsKey(topic + "Count"))
                {
                    Count = (int)(ApplicationData.Current.LocalSettings.Values[topic + "Count"]);
                }
                //If there is nothing yet count the words and save it
                else
                {
                    //Count the words for the first time
                    string filename2 = "Data/" + topic + ".txt";
                    string fileContent2;
                    StorageFile file2 = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename2));
                    using (StreamReader sRead = new StreamReader(await file2.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
                        fileContent2 = await sRead.ReadToEndAsync();
                    //Get the count
                    Count = fileContent2.Split('\n').Length - 1;
                    //Save it for the next time in the local storage
                    ApplicationData.Current.LocalSettings.Values[topic + "Count"] = Count;
                }
                //Set the values for highscore and count to be displayed behind the category name
                TextBlock txtbox = this.FindName(topic+"_1") as TextBlock;
                txtbox.Text = char.ToUpper(topic[0]) + topic.Substring(1) + " (" + HighScore + "/" + Count + ")";
            }         
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Navigate to the details page and give as parameter the name of the category
            var itemId = (e.ClickedItem as StackPanel).Name;
            Frame.Navigate(typeof(TestPageDetails), itemId);
        }
    }
}
