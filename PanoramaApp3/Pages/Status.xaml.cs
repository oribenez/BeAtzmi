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
using Telerik.Windows.Controls;
using SQLite;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;

namespace BeAtzmi.Pages
{
    public partial class Status : PhoneApplicationPage
    {

        InterviewsAppointmentSource appointmentsSource = new InterviewsAppointmentSource();

        public Status()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(FirstLook_Loaded);
        }

        void FirstLook_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayAppointmentsForDate(DateTime.Now.Date);
        }

        ObservableCollection<Tasks> tasksList;

        private void Button_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/AddInterview.xaml", UriKind.Relative));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            var query = dbConn.Table<Tasks>();
            tasksList = new ObservableCollection<Tasks>(await query.ToListAsync());

            if (!showDued.IsChecked.Value)
            {
                tasksList = new ObservableCollection<Tasks>(tasksList.Where(x => x.done == false).ToList());
            }

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            int newGrade = 75, oldGrade = (int)settings["step4Percentage"];
            if (newGrade > oldGrade)
            {
                settings["step4Percentage"] = newGrade;
                settings["bubbleText"] = "כל הכבוד! אתה משתמש במעקב!\nאני מאמין שתמצא עבודה בקרוב...";
            }


            tasksList.OrderBy(si => si.ID);
            tasks.ItemsSource = tasksList;
        }

        private void Button_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/Finish.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/AddNewTask.xaml", UriKind.Relative));
        }

        private async void itemChanged(int id)
        {
            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            int a = await dbConn.UpdateAsync(((ObservableCollection<Tasks>)tasks.ItemsSource).Where(x => x.ID == id).FirstOrDefault());

            if (!showDued.IsChecked.Value)
            {
                tasksList = new ObservableCollection<Tasks>(tasksList.Where(x => x.done == false).ToList());
            }
        }

        private void CheckBox_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            itemChanged(((int)((CheckBox)sender).Tag));
        }

        private void showDued_Unchecked(object sender, RoutedEventArgs e)
        {
            OnNavigatedTo(null);
        }

        private async void RadContextMenuItem_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Tasks selected = selectedTask;
            selectedTask = null;
            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            await dbConn.DeleteAsync(selected);
            tasksList.Remove(selected);
        }

        private void menu_Opening_1(object sender, ContextMenuOpeningEventArgs e)
        {
            Telerik.Windows.Controls.RadDataBoundListBoxItem focusedItem = e.FocusedElement as Telerik.Windows.Controls.RadDataBoundListBoxItem;
            selectedTask = focusedItem.Content as Tasks;
        }

        Tasks selectedTask = null;

        private void calendar_Loaded(object sender, RoutedEventArgs e)
        {
            calendar.AppointmentSource = new InterviewsAppointmentSource();
        }

        private void calendar_SelectedValueChanged(object sender, ValueChangedEventArgs<object> e)
        {
            if (e.NewValue == null)
            {
                this.AppointmentsList.ItemsSource = null;
                return;
            }
            DisplayAppointmentsForDate((e.NewValue as DateTime?).Value);
        }

        private void DisplayAppointmentsForDate(DateTime date)
        {
            appointmentsSource.FetchData(date.Date, date.Date.AddDays(1));
            appointmentsSource.DataLoaded += appointmentsSource_DataLoaded;
        }

        void appointmentsSource_DataLoaded(object sender, EventArgs e)
        {
            if (calendar.SelectedValue.HasValue)
            {
                this.AppointmentsList.ItemsSource = appointmentsSource.GetAppointments((IAppointment appointment) =>
                {
                    DateTime currentAppointmentStart = appointment.StartDate;
                    DateTime currentAppointmentEnd = appointment.EndDate;
                    DateTime requiredAppointmentsStartDate = calendar.SelectedValue.Value.Date;
                    DateTime requiredAppointmentsEndDate = calendar.SelectedValue.Value.Date.AddDays(1);

                    if (requiredAppointmentsEndDate.CompareTo(currentAppointmentStart) >= 0 && requiredAppointmentsStartDate.CompareTo(currentAppointmentEnd) <= 0)
                    {
                        return true;
                    }

                    return false;
                });
            }
            else
            {
                calendar.SelectedValue = (DateTime.Now as DateTime?);
            }
             
        }

        private void AppointmentsList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            int selectedID = ((InterviewAppointment)e.Item.AssociatedDataItem.Value).InterviewData.ID;
            NavigationService.Navigate(new Uri("/Pages/SingleInterview.xaml?ID=" + selectedID.ToString(), UriKind.Relative));
        }
    }
}