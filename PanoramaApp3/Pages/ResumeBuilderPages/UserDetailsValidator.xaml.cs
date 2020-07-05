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

namespace BeAtzmi.Pages.ResumeBuilderPages
{
    public partial class UserDetailsValidator : PhoneApplicationPage
    {
        public UserDetailsValidator()
        {
            InitializeComponent();
        }

        bool isNew = true;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (isNew)
            {
                #region Load Cities

                SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
                city.ItemsSource = (await dbConn.Table<City>().ToListAsync());

                #endregion


                IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

                fname.Text = settings["user_firstname"].ToString();
                lname.Text = settings["user_lastname"].ToString();
                phone.Text = settings["user_phone"].ToString();
                email.Text = settings["user_email"].ToString();
                computerKnoledge.SelectedIndex = (int)settings["user_computer_knowledge"];
                birthdate.Value = (DateTime?)settings["user_birthday"];

                isNew = false;
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if (fname.Text.Replace(" ", "").Length != 0 && lname.Text.Replace(" ", "").Length != 0 && phone.Text.Replace(" ", "").Length != 0 && email.Text.Replace(" ", "").Length != 0 && city.Text.Replace(" ", "").Length != 0 && birthdate.Value.HasValue)
            {
                string[] parameters = { "fname=" + fname.Text, "lname=" + lname.Text, "phone=" + phone.Text, "email=" + email.Text, "birth=" + birthdate.Value.Value.ToBinary().ToString(), "fresh=true", "city=" + city.Text, "computer=" + computerKnoledge.SelectedIndex.ToString() };
                NavigationService.Navigate(new Uri("/Pages/ResumeBuilderPages/ResumeBuilder.xaml?" + String.Join("&", parameters), UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
            else
            {
                MessageBox.Show("חובה למלא את כל השדות!", "שגיאה!", MessageBoxButton.OK);
            }
        }
    }
}