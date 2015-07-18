﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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
    public sealed partial class TestPageDetails : Page
    {
        static Random _random = new Random();

        //Variables
        public string[] EnglishTextArray;
        public string[] NorskTextArray;
        public string Topic;
        public int Count;
        public int Correct;
        public int Current;
        public TestPageDetails()
        {
            this.InitializeComponent();
        }

        //Shuffles two arrays
        static void Shuffle<T>(T[] array1, T[] array2)
        {
            int n = array1.Length;
            for (int i = 0; i < n; i++)
            {
                // Random shuffle
                int r = i + (int)(_random.NextDouble() * (n - i));
                //First Array
                T t1 = array1[r];
                array1[r] = array1[i];
                array1[i] = t1;
                //Second Array
                T t2 = array2[r];
                array2[r] = array2[i];
                array2[i] = t2;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //Read Data
            string filename = "Data/" + e.Parameter.ToString() + ".txt";
            string fileContent;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + filename));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding("iso-8859-1")))
                fileContent = await sRead.ReadToEndAsync();

            titleHeader.Text = "test";

            //Init Variables
            Correct = 0;
            Current = 1;
            Count = fileContent.Split('\n').Length -1;
            EnglishTextArray = new string[Count];
            NorskTextArray = new string[Count];

            //Add Data
            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (titleHeader.Text == "test")
                    {
                        titleHeader.Text = "test - " + line;
                        Topic = line;
                    }
                    else
                    {
                        //Split at tab
                        string[] split = line.Split('\t');
                        EnglishTextArray[i] = split[0];
                        NorskTextArray[i] = split[1];
                        i++;
                    }
                }
            }
            //Shuffle Words
            Shuffle(EnglishTextArray, NorskTextArray);

            //Add information to UI
            counttext.Text = "Word " + Current + " of " + Count;
            scoretext.Text = "Score: " + Correct + "/" + Count;
            englishtext1.Text = EnglishTextArray[0];
            englishtext2.Text = EnglishTextArray[0];
            norsktext.Text = NorskTextArray[0];

            questionGrid.Visibility = Visibility.Visible;
        }

        private void checkAnswer()
        {
            if(String.Equals(answerbox.Text, NorskTextArray[Current - 1],
                   StringComparison.OrdinalIgnoreCase))
            {
                Correct++;
                scoretext.Text = "Score: " + Correct + "/" + Count;
                questionGrid.Visibility = Visibility.Collapsed;
                answerGrid.Visibility = Visibility.Visible;
                feedbacktext2.Visibility = Visibility.Visible;
            }
            else
            {
                feedbacktext.Visibility = Visibility.Visible;             
            }
            removeFeedback();
        }

        private async void removeFeedback()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            feedbacktext.Visibility = Visibility.Collapsed;
            feedbacktext2.Visibility = Visibility.Collapsed;
        }

        private void solvebutton_Click(object sender, RoutedEventArgs e)
        {
            checkAnswer();
        }

        private void solutionbutton_Click(object sender, RoutedEventArgs e)
        {
            questionGrid.Visibility = Visibility.Collapsed;
            answerGrid.Visibility = Visibility.Visible;        
        }

        private void nextbutton_Click(object sender, RoutedEventArgs e)
        {
            //If not the last word
            if(Current != Count)
            {
                Current++;
                //Add information to UI
                counttext.Text = "Word " + Current + " of " + Count;
                englishtext1.Text = EnglishTextArray[Current-1];
                englishtext2.Text = EnglishTextArray[Current-1];
                answerbox.Text = "";
                norsktext.Text = NorskTextArray[Current-1];
                answerGrid.Visibility = Visibility.Collapsed;
                questionGrid.Visibility = Visibility.Visible;
            }
            else
            {
                counttext.Text = "";
                scoretext.Text = "";
                resulttext.Text = "You solved " + Correct + " of " + Count + " correct words.";
                answerGrid.Visibility = Visibility.Collapsed;
                resultGrid.Visibility = Visibility.Visible;
            }
            
        }

        private async void listenbutton_Click(object sender, RoutedEventArgs e)
        {
            //Build url string
            var urlPart1 = "http://translate.google.de/translate_tts?ie=UTF-8&q=";
            var input = norsktext.Text;
            var inputlength = input.Length;
            input = input.Replace(" ", "%20");
            var urlPart2 = "&tl=no&total=1&idx=0&textlen=";
            var urlPart3 = "&tk=171372&client=t&prev=input";
            var url = urlPart1 + input + urlPart2 + inputlength + urlPart3;
            //Call url
            mediaelement.Source = new Uri(url);
            
            //Backup for IE
            //var uri = new Uri(url);
            //await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void answerbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //If enter key is pressed
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                checkAnswer();
            }
        }
    }
}
