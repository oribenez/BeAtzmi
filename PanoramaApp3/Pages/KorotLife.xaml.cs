using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace BeAtzmi
{
    public partial class KorotLife : PhoneApplicationPage
    {
        public KorotLife()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Interview.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (((int)settings["step1Percentage"]) != 100)
            {
                settings["step1Percentage"] = 100;
                settings["bubbleText"] = "עכשיו, כשאתה יודע איך לבנות קו\"ח\nאפשר להתחיל לעבוד באמת...";
                settings.Save();
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ResumeBuilderPages/UserDetailsValidator.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }
    }
}