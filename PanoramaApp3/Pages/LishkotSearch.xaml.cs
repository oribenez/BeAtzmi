using BeAtzmi.Classes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Windows.Devices.Geolocation;
using Windows.Phone.Speech.Synthesis;

namespace BeAtzmi.Pages
{
    public partial class LishkotSearch : PhoneApplicationPage
    {
        public LishkotSearch()
        {
            InitializeComponent();
        }

        private SQLiteAsyncConnection dbConn;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await sortByDistance();   
        }

        ObservableCollection<Lishkot> result;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            progressIndicator.IsVisible = true;
            searchBox.Text = String.Empty;
            ContentPanel.Visibility = System.Windows.Visibility.Collapsed;
            dbConn = new SQLiteAsyncConnection(App.DB_PATH);

            SpeechSynthesizer synth = new SpeechSynthesizer();

            if (NavigationContext.QueryString.ContainsKey("voiceCommandName"))
            {
                await synth.SpeakTextAsync("Locating your phone...");
            }

            var query = dbConn.Table<Lishkot>();
            result = new ObservableCollection<Lishkot>(await query.ToListAsync());

            await sortByDistance();

            ContentPanel.Visibility = System.Windows.Visibility.Visible;
            progressIndicator.IsVisible = false;
            ContentPanel.DataContext = result;

            if (NavigationContext.QueryString.ContainsKey("voiceCommandName"))
            {
                string voicecommandname = NavigationContext.QueryString["voiceCommandName"];

                NavigationContext.QueryString.Remove("voiceCommandName");

                if (voicecommandname == "CallBureau")
                {
                    PhoneCallTask phoneCallTask = new PhoneCallTask();

                    phoneCallTask.PhoneNumber = result[0].phone;
                    phoneCallTask.DisplayName = "לשכת תעסוקה " + result[0].name;



                    await synth.SpeakTextAsync("Calling to the closest employment bureau. Please confirm call.");

                    phoneCallTask.Show();


                }
                else if (voicecommandname == "ClosestBureau")
                {
                    await synth.SpeakTextAsync("Showing the closest employment bureau.");
                    NavigationService.Navigate(new Uri("/Pages/SingleLishka.xaml?name=" + result[0].name, UriKind.Relative));
                }
            }

        }

        private async Task sortByDistance()
        {
            double lat = 0, lon = 0;

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("gpsEnabled") || !((bool)settings["gpsEnabled"]))
            {
                settings.Remove("gpsEnabled");

                MessageBoxResult res = MessageBox.Show("שימוש במיקום יסייע לך למצוא את לשכת התעסוקה הקרובה ביותר למקום משכנך.", "האם ברצונך לאפשר לאפליקציה להשתמש במיקומך הנוכחי?", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    settings.Add("gpsEnabled", true);
                }
                else
                {
                    settings.Add("gpsEnabled", false);
                }
                settings.Save();

                if (res != MessageBoxResult.OK) return;
            }

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 200;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                lat = geoposition.Coordinate.Latitude;
                lon = geoposition.Coordinate.Longitude;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    MessageBox.Show("שירותי מיקום מושבתים במכשיר זה!", "שגיאה!", MessageBoxButton.OK);
                }
                //else
                {
                    MessageBox.Show("אין אפשרות לקבוע מיקום!", "שגיאה!", MessageBoxButton.OK);
                }
            }

            if (lat == 0 && lon == 0) return;

            List<Lishkot> lishkotNew = new List<Lishkot>(result);

            foreach (var lishka in lishkotNew)
            {
                string[] loc = lishka.location.Split(',');
                double Lishka_lat = double.Parse(loc[0]);
                double Lishka_lon = double.Parse(loc[1]);
                lishka.distance = distance(lat, lon, Lishka_lat, Lishka_lon, 'K');
            }

            lishkotNew.Sort(
                delegate(Lishkot x, Lishkot y)
                {
                    if (x == null)
                    {
                        if (y == null) { return 0; }
                        return -1;
                    }
                    if (y == null) { return 0; }
                    return x.distance.CompareTo(y.distance);
                });


            result.Clear();
            foreach (Lishkot lish in lishkotNew)
            {
                result.Add(lish);
            }
            lishkotNew.Clear();

        }
        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) +
                          Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
                          Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private void searchBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try { itemSelected(((Lishkot)searchBox.SelectedItem).name); }
            catch { }
        }

        private void itemSelected(string p)
        {
            NavigationService.Navigate(new Uri("/Pages/SingleLishka.xaml?name=" + p, UriKind.Relative));
        }

        private void lishkot_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            itemSelected(((Lishkot)lishkot.SelectedItem).name);
        }

        private void searchBox_TextInputStart(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }

        private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //((AutoCompleteBox)sender).Text = String.Empty;
        }
    }
}