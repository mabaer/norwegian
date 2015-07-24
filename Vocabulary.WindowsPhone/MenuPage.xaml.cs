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
using System.Threading.Tasks;

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
    public sealed partial class MenuPage : Page
    {
        public MenuPage()
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
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PracticePage));
        }

        private void Grid_Tapped2(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TestPage));
        }

        private void Grid_Tapped3(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ReadPage));
        }

        private void Grid_Tapped4(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PronouncePage));
        }

        private async void Grid_Tapped5(object sender, TappedRoutedEventArgs e)
        {
            //Enable Progressbar and wait a static time of 1 second for loading of the rss feed
            progressBar.Visibility = Visibility.Visible;
            await Task.Delay(TimeSpan.FromSeconds(1));
            Frame.Navigate(typeof(MainPage));
        }

        private void Grid_Tapped6(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void about_click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutThisAppPage));
        }

        private async void privacy_click(object sender, RoutedEventArgs e)
        {
            //Navigate to the privacy url file
            string uriToLaunch = @"http://1drv.ms/1Rx7S9r";
            var uri = new Uri(uriToLaunch);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
