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
using System.IO;
using System.Text;
using Microsoft.Phone.Tasks;

namespace BeAtzmi.Pages
{
    public partial class SingleService : PhoneApplicationPage
    {
        public SingleService()
        {
            InitializeComponent();
        }

        Service curentService;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            int selectedID = int.Parse(NavigationContext.QueryString["serviceID"]);

            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            var query = dbConn.Table<Service>();
            curentService = (await query.ToListAsync()).Where(service => service.ID == selectedID).FirstOrDefault();

            TextReader input = new StreamReader(Application.GetResourceStream(new Uri("Assets/ServiceDescription/" + curentService.infoFileName + ".txt", UriKind.Relative)).Stream, Encoding.UTF8);
            curentService.description = input.ReadToEnd();

            LayoutRoot.DataContext = curentService;

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            int newGrade = 100, oldGrade = (int)settings["step3Percentage"];
            if (newGrade > oldGrade) settings["step3Percentage"] = newGrade;
            settings["bubbleText"] = "טוב מאוד! מצאת שירות שיעזור לך.\nלדעתי, אתה מוכן! תתחיל לחפש\nעבודות ותשתמש במעקב שלנו...";

            if (curentService.address != null && curentService.address.IndexOf("http") == 0)
            {
                regular.Visibility = System.Windows.Visibility.Collapsed;
                linkButton.Visibility = System.Windows.Visibility.Visible;

                linkButton.NavigateUri = new Uri(curentService.address, UriKind.Absolute);
            }
            else
            {
                regular.Visibility = System.Windows.Visibility.Visible;
                linkButton.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (curentService.email != null && curentService.email.Length > 0)
            {
                mailBtn.NavigateUri = new Uri("mailto:" + curentService.email, UriKind.Absolute);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (curentService.phone == null || curentService.phone.Length == 0) return;

            PhoneCallTask phoneCallTask = new PhoneCallTask();

            phoneCallTask.PhoneNumber = curentService.phone;
            phoneCallTask.DisplayName = curentService.name;

            phoneCallTask.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (curentService.phone == null || curentService.phone.Length == 0) return;

            SaveContactTask saveContactTask = new SaveContactTask();

            saveContactTask.FirstName = curentService.name;
            saveContactTask.LastName = "";
            saveContactTask.MobilePhone = curentService.phone;

            saveContactTask.Show();
        }

    }
}