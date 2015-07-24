using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage;
using System.Text;

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
    //Class to Representing a vocabulary image pair for the flipview
    public class VocabularyPractice
    {
        public string Translation { get; set; }
        public string Image { get; set; }
        public VocabularyPractice(string t, string i)
        {
            Translation = t;
            Image = i;
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PracticePageDetails : Page
    {
        public PracticePageDetails()
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
            //Clear Flipview
            flipView1.Items.Clear();

            //Read Data from the vocabulary file
            string filename = "Data/" + e.Parameter.ToString() + ".txt";
            string fileContent;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///"+filename));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
            fileContent = await sRead.ReadToEndAsync();

            titleHeader.Text = "practice";
            Random rnd = new Random();

            //Add Data
            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    //In the first iteration just add the category name to the header
                    if(titleHeader.Text == "practice")
                    {
                        titleHeader.Text = "practice - " + line;
                    }
                    else
                    {
                        //Split at tab
                        string[] split = line.Split('\t');
                        //Get a random picture number
                        int pic = rnd.Next(1, 18);
                        flipView1.Items.Add(new VocabularyPractice(split[0] + " - " + split[1], "Assets/BackgroundImage/Img" + pic + ".jpg"));
                    }
                }
            } 
        }
    }
}
