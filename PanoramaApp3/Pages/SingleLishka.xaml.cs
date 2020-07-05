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
using Microsoft.Phone.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;

namespace BeAtzmi.Pages
{
    public partial class SingleLishka : PhoneApplicationPage
    {
        public SingleLishka()
        {
            InitializeComponent();
        }

        string name;
        private SQLiteAsyncConnection dbConn;
        Lishkot single;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LayoutRoot.Visibility = System.Windows.Visibility.Collapsed;
            name = NavigationContext.QueryString["name"];

            dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            var query = dbConn.Table<Lishkot>();
            single = await query.Where((Lishkot l) => l.name == name).FirstOrDefaultAsync();
            LayoutRoot.DataContext = single;

            LayoutRoot.Visibility = System.Windows.Visibility.Visible;
        }

        private void CallChamb_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();

            phoneCallTask.PhoneNumber = (string)((Border)sender).Tag;
            phoneCallTask.DisplayName = "לשכת תעסוקה " + name;

            phoneCallTask.Show();
        }

        private void LinkWeb_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri((string)((Border)sender).Tag, UriKind.Absolute);

            webBrowserTask.Show();
        }

        private void Image_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SaveContactTask saveContactTask = new SaveContactTask();

            saveContactTask.FirstName = "לשכת תעסוקה " + name;
            saveContactTask.LastName = "";
            saveContactTask.MobilePhone = ((Lishkot)LayoutRoot.DataContext).phone;

            saveContactTask.Show();
        }

        private void Map_Loaded_1(object sender, RoutedEventArgs e)
        {
            #warning "On deploy change to real certificates"
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "ApplicationID";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "AuthenticationToken";

            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);

            double lat, lon;
            string[] loc = single.location.Split(',');
            lat = double.Parse(loc[0]);
            lon = double.Parse(loc[1]);

            myLocationOverlay.GeoCoordinate = new System.Device.Location.GeoCoordinate(lat, lon);

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);

            // Add the MapLayer to the Map.
            map.Layers.Add(myLocationLayer);
            map.Center = new System.Device.Location.GeoCoordinate(lat, lon);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MapsDirectionsTask mapsDirectionsTask = new MapsDirectionsTask();

            // You can specify a label and a geocoordinate for the end point.
            // GeoCoordinate spaceNeedleLocation = new GeoCoordinate(47.6204,-122.3493);
            // LabeledMapLocation spaceNeedleLML = new LabeledMapLocation("Space Needle", spaceNeedleLocation);

            // If you set the geocoordinate parameter to null, the label parameter is used as a search term.
            string lishkaName = "לשכת תעסוקה " + single.name;
            double lat, lon;
            string[] loc = single.location.Split(',');
            lat = double.Parse(loc[0]);
            lon = double.Parse(loc[1]);
            LabeledMapLocation spaceNeedleLML = new LabeledMapLocation(lishkaName, new System.Device.Location.GeoCoordinate(lat, lon));

            mapsDirectionsTask.End = spaceNeedleLML;

            // If mapsDirectionsTask.Start is not set, the user's current location is used as the start point.

            mapsDirectionsTask.Show();
        }
    }
}