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

namespace BeAtzmi.Pages
{
    public partial class AddNewTask : PhoneApplicationPage
    {
        public AddNewTask()
        {
            InitializeComponent();
        }

        private async void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (taskName.Text.Length == 0) return;

            Tasks task = new Tasks
            {
                text = taskName.Text,
                done = false,
                dueDate = date.Value.Value.Date.Add(time.Value.Value.TimeOfDay)
            };

            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            await dbConn.InsertAsync(task);

            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }
    }
}