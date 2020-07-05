using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BeAtzmi.Classes;
using SQLite;
using Microsoft.Phone.Tasks;

namespace BeAtzmi.Pages
{
    public partial class SingleInterview : PhoneApplicationPage
    {
        public SingleInterview()
        {
            InitializeComponent();
        }

        Interviews single;
        SQLiteAsyncConnection dbConn;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            int selectedID = int.Parse(NavigationContext.QueryString["ID"]);

            dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            single = (await dbConn.Table<Interviews>().ToListAsync()).Where(x => x.ID == selectedID).FirstOrDefault();
            LayoutRoot.DataContext = single;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();

            phoneCallTask.PhoneNumber = single.ContactPhone.ToString();
            phoneCallTask.DisplayName = single.ContactName + " - " + single.CompanyName;

            phoneCallTask.Show();
        }

        private async void Button_Click_1(object sender, EventArgs e)
        {
            await dbConn.DeleteAsync(((await dbConn.Table<Interviews>().ToListAsync()).Where(x => x.ID == single.ID).FirstOrDefault()));
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }
    }
} 