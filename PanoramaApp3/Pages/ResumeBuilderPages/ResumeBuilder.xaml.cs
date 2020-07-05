using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BeAtzmi.Classes.ResumeBuilderItems;
using Microsoft.Phone.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace BeAtzmi.Pages.ResumeBuilderPages
{
    public partial class ResumeBuilder : PhoneApplicationPage
    {
        public ResumeBuilder()
        {
            InitializeComponent();
        }

        private ResumeUser _currentUser;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(NavigationContext.QueryString.ContainsKey("fresh"))
            {
                DateTime date = DateTime.FromBinary(long.Parse(NavigationContext.QueryString["birth"]));
                _currentUser = new ResumeUser(NavigationContext.QueryString["fname"], NavigationContext.QueryString["lname"], NavigationContext.QueryString["email"], NavigationContext.QueryString["phone"], NavigationContext.QueryString["city"], date, int.Parse(NavigationContext.QueryString["computer"]));

                NavigationContext.QueryString.Remove("fresh");
                NavigationContext.QueryString.Remove("fname");
                NavigationContext.QueryString.Remove("lname");
                NavigationContext.QueryString.Remove("email");
                NavigationContext.QueryString.Remove("phone");
                NavigationContext.QueryString.Remove("city");
                NavigationContext.QueryString.Remove("computer");
            }

            if (PhoneApplicationService.Current.State.ContainsKey("resumeItem") && PhoneApplicationService.Current.State["resumeItem"] != null)
            {
                object item = PhoneApplicationService.Current.State["resumeItem"];
                AddItem((IResumeBuilderItem)item);
                PhoneApplicationService.Current.State["resumeItem"] = null;
            }
        }

        private void AddItem(IResumeBuilderItem item)
        {
            _currentUser.AddItem(item);
            list.ItemsSource = _currentUser.Items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ResumeBuilderPages/ItemCreatorPages/AddLanguage.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ResumeBuilderPages/ItemCreatorPages/AddMilitaryService.xaml", UriKind.Relative));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ResumeBuilderPages/ItemCreatorPages/AddWorkExperience.xaml", UriKind.Relative));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ResumeBuilderPages/ItemCreatorPages/AddEducation.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            bool OK = true;
            for (int i = 1; i <= 4; i++)
            {
                ResumeBuilderItemKind kind = (ResumeBuilderItemKind)i;
                if (_currentUser.Items.Where(x => x.Kind == kind).Count() == 0) OK = false;
            }

            if (!OK)
            {
                MessageBox.Show("חובה להזין לפחות אחד מכל סוג של פריט מידע אפשרי.", "תקלה!", MessageBoxButton.OK);
                return;
            }

            string fileName = "Resume.docx";
            _currentUser.WriteUserResumeToFile(fileName);
            NavigationService.Navigate(new Uri("/Pages/ResumeBuilderPages/PublishFile.xaml?filePath=" + fileName, UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }

        
    }
}