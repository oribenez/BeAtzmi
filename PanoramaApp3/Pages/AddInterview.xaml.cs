using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using BeAtzmi.Classes;
using SQLite;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace BeAtzmi.Pages
{
    public partial class AddInterview : PhoneApplicationPage
    {
        public AddInterview()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (shouldReturnOnNavgiatedTo)
            {
                shouldReturnOnNavgiatedTo = false;
                navigateBack();
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (!validate()) return;

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            int newGrade = 100, oldGrade = (int)settings["step4Percentage"];
            if (newGrade > oldGrade)
            {
                settings["step4Percentage"] = newGrade;
                settings["bubbleText"] = "בהצלחה בראיון עבודה שלך ב:\n" + name.Text;
            }

            if (saveContact.IsChecked.Value)
            {
                doContact();
            }
            else
            {
                showAppoint();
            }
        }

        private async void showAppoint()
        {
            shouldReturnOnNavgiatedTo = true;
            await doAppointment();
        }

        public bool shouldReturnOnNavgiatedTo = false;

        private bool validate()
        {
            if (name.Text.Replace(" ", "").Length == 0 || address.Text.Replace(" ", "").Length == 0 || contactName.Text.Replace(" ", "").Length == 0 || contactPhone.Text.Replace(" ", "").Length == 0)
            {
                MessageBox.Show("חובה למלא את כל השדות!", "שגיאה!", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        void saveContactTask_Completed(object sender, SaveContactResult e)
        {
            switch (e.TaskResult)
            {
                //Logic for when the contact was saved successfully
                case TaskResult.OK:
                    MessageBox.Show("איש הקשר נשמר.");
                    break;

                //Logic for when the task was cancelled by the user
                case TaskResult.Cancel:
                    MessageBox.Show("שמירה בוטלה.");
                    break;

                //Logic for when the contact could not be saved
                case TaskResult.None:
                    MessageBox.Show("אין אפשרות לשמור את איש הקשר.");
                    break;
            }

            showAppoint();
        }

        async Task doAppointment()
        {
            if (setAppointement.IsChecked.Value)
            {
                SaveAppointmentTask saveAppointmentTask = new SaveAppointmentTask();

                saveAppointmentTask.StartTime = date.Value.Value.Date.Add(time.Value.Value.TimeOfDay);
                saveAppointmentTask.EndTime = saveAppointmentTask.StartTime.Value.AddMinutes(45);
                saveAppointmentTask.Subject = "ראיון עבודה - " + name.Text;
                saveAppointmentTask.Location = address.Text;
                saveAppointmentTask.Details = "איש קשר:" + Environment.NewLine + contactName.Text + " - " + contactPhone.Text;
                saveAppointmentTask.IsAllDayEvent = false;
                saveAppointmentTask.Reminder = Reminder.ThirtyMinutes;
                saveAppointmentTask.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;

                saveAppointmentTask.Show();
            }

            await saveToSQLITE();
        }

        void doContact()
        {
            if (saveContact.IsChecked.Value)
            {
                SaveContactTask saveContactTask = new SaveContactTask();

                saveContactTask.FirstName = contactName.Text + " - " + name.Text;
                saveContactTask.LastName = "";
                saveContactTask.MobilePhone = contactPhone.Text;

                saveContactTask.Show();

                saveContactTask.Completed += new EventHandler<SaveContactResult>(saveContactTask_Completed);
            }
        }

        void navigateBack()
        {
            NavigationService.Navigate(new Uri("/Pages/Status.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }

        async Task saveToSQLITE()
        {
            // Create a new task.
            Interviews interview = new Interviews()
            {
                CompanyName = name.Text,
                Address = address.Text,
                Datetime = date.Value.Value.Date.Add(time.Value.Value.TimeOfDay),
                ContactName = contactName.Text,
                ContactPhone = contactPhone.Text
            };

            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            await dbConn.InsertAsync(interview);

            if (!saveContact.IsChecked.Value && !setAppointement.IsChecked.Value)
            {
                navigateBack();
            }
        }
    }
}