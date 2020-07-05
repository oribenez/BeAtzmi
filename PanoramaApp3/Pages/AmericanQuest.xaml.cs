using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SQLite;
using BeAtzmi.Classes;
using System.IO.IsolatedStorage;

namespace BeAtzmi.Pages
{
    public partial class AmericanQuest : PhoneApplicationPage
    {
        public AmericanQuest()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private SQLiteAsyncConnection dbConn;
        int _ID, selectedAnswer = -1, _counter, _total;
        string nextQuestions;
        Questions single;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this._ID = int.Parse(NavigationContext.QueryString["ID"]);
            this._total = int.Parse(NavigationContext.QueryString["total"]);
            this.nextQuestions = NavigationContext.QueryString["nextQuestions"];
            this._counter = int.Parse(NavigationContext.QueryString["counter"]);
            dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            var query = dbConn.Table<Questions>();
            single = await query.Where((Questions q) => q.ID == this._ID).FirstOrDefaultAsync();

            Random rnd = new Random();

            int loc1, loc2, loc3, loc4;
            loc2 = loc3 = loc4 = loc1 = rnd.Next(1, 5);

            while (loc2 == loc1)
            {
                loc2 = rnd.Next(1, 5);
            }

            while (loc3 == loc1 || loc3 == loc2)
            {
                loc3 = rnd.Next(1, 5);
            }

            loc4 = 10 - loc1 - loc2 - loc3;


            LayoutRoot.DataContext = single;
            Ans1.SetValue(Grid.RowProperty, loc1 - 1);
            Ans2.SetValue(Grid.RowProperty, loc2 - 1);
            Ans3.SetValue(Grid.RowProperty, loc3 - 1);
            Ans4.SetValue(Grid.RowProperty, loc4 - 1);

            questionRef.Text = String.Format("שאלה {0} מתוך {1}", _counter, _total);
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {

        }

        private void Ans1_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.HasValue && ((RadioButton)sender).IsChecked.Value)
                selectedAnswer = 1;
        }

        private void Ans2_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.HasValue && ((RadioButton)sender).IsChecked.Value)
                selectedAnswer = 2;
        }

        private void Ans3_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.HasValue && ((RadioButton)sender).IsChecked.Value)
                selectedAnswer = 3;
        }

        private void Ans4_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.HasValue && ((RadioButton)sender).IsChecked.Value)
                selectedAnswer = 4;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAnswer == -1)
            {
                MessageBox.Show("חובה לבחור תשובה לשאלה!", "שגיאה!", MessageBoxButton.OK);
                return;
            }

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("answers"))
                settings.Add("answers", "");

            settings["answers"] += _ID.ToString() + "-" + selectedAnswer.ToString() + ";";
            settings.Save();

            if (nextQuestions != "")
            {
                string[] nextArray = nextQuestions.Split(',');

                int newID = int.Parse(nextArray[0]);
                nextQuestions = "";

                for (int i = 1; i < nextArray.Length; i++)
                {
                    nextQuestions += "," + nextArray[i].ToString();
                }


                if (nextQuestions.Length > 0) nextQuestions = nextQuestions.Substring(1);

                _counter++;
                string uri = String.Format("/Pages/AmericanQuest.xaml?ID={0}&counter={1}&nextQuestions={2}&total={3}", newID, _counter, nextQuestions, _total);
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
            else
            {
                proccessAndShowFeedback(settings["answers"]);
            }
        }

        private void proccessAndShowFeedback(object p)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings.Remove("answers");
            settings.Save();
            NavigationService.Navigate(new Uri("/Pages/goodJob1.xaml?total=" + _total + "&p=" + p.ToString(), UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }
    }
}