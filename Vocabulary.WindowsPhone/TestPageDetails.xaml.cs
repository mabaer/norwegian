using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
    public sealed partial class TestPageDetails : Page
    {
        //Rng for shuffeling the words
        static Random _random = new Random();

        //Variables
        public string[] EnglishTextArray;
        public string[] NorskTextArray;
        public string Topic;
        public int Count;
        public int Correct;
        public int Current;
        public string[] EnglishTextArray2;
        public string[] NorskTextArray2;
        public bool isRecap;
        public int wrongCount;
        public TestPageDetails()
        {
            this.InitializeComponent();
        }

        //Shuffles two arrays individually with the same pattern
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
            EnglishTextArray2 = new string[Count];
            NorskTextArray2 = new string[Count];
            isRecap = false;
            wrongCount = 0;

            //Add Data
            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    //In the first iteration just add the name of the category to the header
                    if (titleHeader.Text == "test")
                    {
                        titleHeader.Text = "test - " + line;
                        Topic = line;
                    }
                    else
                    {
                        //Split at tab and add the english and the norwegian word to the array
                        string[] split = line.Split('\t');
                        EnglishTextArray[i] = split[0];
                        NorskTextArray[i] = split[1];
                        i++;
                    }
                }
            }
            //Shuffle Words
            Shuffle(EnglishTextArray, NorskTextArray);

            //Add information to UI for the first word
            counttext.Text = "Word " + Current + " of " + Count;
            scoretext.Text = "Score: " + Correct + "/" + Count;
            englishtext1.Text = EnglishTextArray[0];
            englishtext2.Text = EnglishTextArray[0];
            norsktext.Text = NorskTextArray[0];

            //Display the question grid
            questionGrid.Visibility = Visibility.Visible;
        }

        private void checkAnswer()
        {
            //If the string is the same (non-case-sensitiv) accept the input 
            if(String.Equals(answerbox.Text, norsktext.Text,
                   StringComparison.OrdinalIgnoreCase))
            {
                //If it is the challenge mode increment the score
                if(!isRecap)
                {
                    Correct++;
                    scoretext.Text = "Score: " + Correct + "/" + Count;
                }
                //Switch to the answer grid
                questionGrid.Visibility = Visibility.Collapsed;
                answerGrid.Visibility = Visibility.Visible;
                feedbacktext2.Visibility = Visibility.Visible;
            }
            else
            {
                //If the answer is wrong display the feedback text
                feedbacktext.Visibility = Visibility.Visible;             
            }
            removeFeedback();
        }

        //Removes the Feedback texts after 2 seconds
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
            //If it is the challenge mode add the wrong solved word to the arrays for the recap
            if(!isRecap)
            {
                EnglishTextArray2[wrongCount] = englishtext1.Text;
                NorskTextArray2[wrongCount] = norsktext.Text;
                wrongCount++;
            } 
            //Switch to answer grid
            questionGrid.Visibility = Visibility.Collapsed;
            answerGrid.Visibility = Visibility.Visible;        
        }

        private void nextbutton_Click(object sender, RoutedEventArgs e)
        {
            //If it is not the last word
            if(Current != Count)
            {
                Current++;
                //Add information to UI
                counttext.Text = "Word " + Current + " of " + Count;
                //If it is the recap take the next word from the recap array
                if(isRecap)
                {
                    englishtext1.Text = EnglishTextArray2[Current - 1];
                    englishtext2.Text = EnglishTextArray2[Current - 1];
                    norsktext.Text = NorskTextArray2[Current - 1];
                }
                //Otherwise from the regular arrays
                else
                {
                    englishtext1.Text = EnglishTextArray[Current - 1];
                    englishtext2.Text = EnglishTextArray[Current - 1];
                    norsktext.Text = NorskTextArray[Current - 1];
                }
                answerbox.Text = "";
                //Switch to question grid
                answerGrid.Visibility = Visibility.Collapsed;
                questionGrid.Visibility = Visibility.Visible;
            }
            else
            {
                //Recap case.
                if(isRecap)
                {
                    counttext.Text = "";
                    scoretext.Text = "";
                    resulttext.Text = "You finished the recap.";
                    resulttext2.Text = "";
                    repeatButton.Visibility = Visibility.Collapsed;
                    answerGrid.Visibility = Visibility.Collapsed;
                    resultGrid.Visibility = Visibility.Visible;
                    return;
                }
                //Normal end
                counttext.Text = "";
                scoretext.Text = "";
                resulttext.Text = "You solved " + Correct + " of " + Count + " correct words.";
                resulttext2.Text = "";
                //If there is already a highscroe check if it is a new one
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Topic))
                {
                    int HighScore;
                    HighScore = (int)(ApplicationData.Current.LocalSettings.Values[Topic]);
                    if(HighScore < Correct)
                    {
                        resulttext2.Text = "Congratulation!!! You got a new highscore!";
                        ApplicationData.Current.LocalSettings.Values[Topic] = Correct;
                        ApplicationData.Current.LocalSettings.Values[Topic + "Count"] = Count;
                    }
                }
                //Otherwise it is a new highscore anyway
                else
                {
                    resulttext2.Text = "Congratulation!!! You got a new highscore!";
                    ApplicationData.Current.LocalSettings.Values[Topic] = Correct;
                    ApplicationData.Current.LocalSettings.Values[Topic+"Count"] = Count;
                }
                //If not all words are coorectly solved show the recap button
                if(Correct != Count)
                {
                    repeatButton.Visibility = Visibility.Visible;
                }
                else
                {
                    repeatButton.Visibility = Visibility.Collapsed;
                }
                //Switch to result grid
                answerGrid.Visibility = Visibility.Collapsed;
                resultGrid.Visibility = Visibility.Visible;
            }
            
        }

        private void listenbutton_Click(object sender, RoutedEventArgs e)
        {
            //Build url string to read out the word
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

        private void repeatButton_Click(object sender, RoutedEventArgs e)
        {
            //Reset everything for the recap
            isRecap = true;
            Count = wrongCount;
            Current = 1;
            englishtext1.Text = EnglishTextArray2[Current - 1];
            englishtext2.Text = EnglishTextArray2[Current - 1];
            norsktext.Text = NorskTextArray2[Current - 1];
            answerbox.Text = "";
            counttext.Text = "Word " + Current + " of " + Count;
            answerGrid.Visibility = Visibility.Collapsed;
            resultGrid.Visibility = Visibility.Collapsed;
            questionGrid.Visibility = Visibility.Visible;
        }
    }
}
