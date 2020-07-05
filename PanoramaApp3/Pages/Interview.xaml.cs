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
using SQLite;
using BeAtzmi.Classes;

namespace BeAtzmi
{
    public partial class Interview : PhoneApplicationPage
    {
        public Interview()
        {
            InitializeComponent();
        }

        const int NUMBER_OF_QUESTIONS = 10;

        private async void showQuestions()
        {
            //Step 2
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("answers")) settings.Remove("answers");
            settings.Add("answers", "");
            settings.Save();

            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            var query = dbConn.Table<Questions>();
            int count = await query.CountAsync();

            var rng = new Random();
            var values = Enumerable.Range(1, count).OrderBy(x => rng.Next()).ToArray();
            string questions = "";
            for (int i = 1; i < NUMBER_OF_QUESTIONS; i++)
            {
                questions += "," + values[i].ToString();
            }

            string uri = String.Format("/Pages/AmericanQuest.xaml?ID={0}&counter={1}&nextQuestions={2}&total={3}", values[0], 1, questions.Substring(1), NUMBER_OF_QUESTIONS);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (((int)settings["step2Percentage"]) < 16)
            {
                settings["step2Percentage"] = 16;
                settings["bubbleText"] = "נסה את המבחן האינטראקטבי\nלראיון העבודה";
                settings.Save();
            }
            
        }

        private void Button_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            showQuestions();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Knowledge.xaml", UriKind.Relative));
        }
    }
}