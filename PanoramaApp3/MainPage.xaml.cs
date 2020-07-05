using BeAtzmi.Classes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace BeAtzmi
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            var taskName = "TileUpdateTask";

            // If the task exists
            var oldTask = ScheduledActionService.Find(taskName) as PeriodicTask;
            if (oldTask != null)
            {
                ScheduledActionService.Remove(taskName);
            }

            // Create the Task
            PeriodicTask task = new PeriodicTask(taskName);

            // Description is required
            task.Description = "Update tips on Be-Atzmi App.";

            // Add it to the service to execute
            ScheduledActionService.Add(task);

            #warning Remove this on release
            ScheduledActionService.LaunchForTest(taskName, TimeSpan.FromMilliseconds(1500));
        }

        // Load data for the ViewModel Items
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            double percentage = ((int)settings["step1Percentage"]) + ((int)settings["step2Percentage"]) + ((int)settings["step3Percentage"]) + ((int)settings["step4Percentage"]);
            percentage /= 4;
            bar.Value = percentage;
            bubble.Content = settings["bubbleText"];
            
            if (NavigationContext.QueryString.ContainsKey("callTaasokav"))
            {
                NavigationContext.QueryString.Remove("callTaasokav");
                CallTasokavAppBarButton_Click(null, null);
            }

            if(!settings.Contains("user_firstname"))
            {
                NavigationService.Navigate(new Uri("/Pages/Settings.xaml?mand=1", UriKind.Relative));
            }

            tip.Text = getRandomTip();
        }

        private string getRandomTip()
        {
            XDocument xml = XDocument.Load(Application.GetResourceStream(new Uri("Tips.xml", UriKind.Relative)).Stream);

            var q = xml.Element("Tips").Descendants("Tip");

            return q.ElementAt((new Random()).Next(0, q.Count())).Value;
        }

 
        private void CallTasokavAppBarButton_Click(object sender, EventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();

            phoneCallTask.PhoneNumber = "*2119";
            phoneCallTask.DisplayName = "תעסו-קו";

            phoneCallTask.Show();
        }

        private void btnSettingsAppBar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
        }

        //private void Status_MouseLeftButtonDown(object sender, EventArgs e)
        //{
        //    ScaleTransform Scale = new ScaleTransform();
        //    Scale.ScaleX = 0.9;
        //    Scale.ScaleY = 0.9;
            
        //}
        //private void Status_Click(object sender, EventArgs e)
        //{
        //    NavigationService.Navigate(new Uri("/Pages/Calender.xaml", UriKind.Relative));
        //}

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/LishkotSearch.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/LishkotSearch.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_3(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Contact.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_4(object sender, EventArgs e)
        {
            ApplicationBarIconButton_Click_3(sender, e);
        }

        private void Interview_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Interview.xaml", UriKind.Relative));
        }

        private void KorotLife_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/KorotLife.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            var n = from c in ShellTile.ActiveTiles
                    where c.NavigationUri.ToString() == "/MainPage.xaml?callTaasokav=true"
                    select c;

            if (n.Count() == 0)
            {
                FlipTileData tileData = new FlipTileData()
                {
                    Title = "",
                    //BackgroundColor = Color.FromArgb(255, 99, 135, 166),
                    BackBackgroundImage = new Uri("/Assets/Tiles/backMediumBlue.png", UriKind.Relative),
                    BackContent = "נסה להתקשר לתעסו-קו",
                    BackgroundImage = new Uri("/Assets/Tiles/MediumCallIcon.png", UriKind.Relative),
                    SmallBackgroundImage = new Uri("/Assets/Tiles/SmallCallIcon.png", UriKind.Relative)
                };

                ShellTile.Create(new Uri("/MainPage.xaml?callTaasokav=true", UriKind.Relative), tileData, false);
            }
            else
            {
                MessageBox.Show("כבר קיים חיוג מהיר בדף הבית!", "שגיאה!", MessageBoxButton.OK);
            }
        }

        private void Status_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Status.xaml", UriKind.Relative));
        }

        private void Knowledge_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Knowledge.xaml", UriKind.Relative));
        }

        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SuccessStory1.xaml", UriKind.Relative));
        }

        private void Border_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SuccessStory2.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("", "האם אתה בטוח שמצאת עבודה?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                NavigationService.Navigate(new Uri("/Pages/Finish.xaml", UriKind.Relative));
            }
        }

        private void Button_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

    }
}