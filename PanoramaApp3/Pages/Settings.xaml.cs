using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace BeAtzmi.Pages
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();

           // familyState.ItemsSource = new List<string> { "רווק/ה", "נשוי/ה", "אלמנ/ה", "פרוד/ה", "גר/ה עם בן/בת זוג (לא נשוי)", "ידוע/ה בציבור", "נטוש/ה" };
            familyState.SetValue(ListPicker.ItemCountThresholdProperty, 10);

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("user_firstname"))
            {
                firstName.Text = settings["user_firstname"].ToString();
                lastName.Text = settings["user_lastname"].ToString();
                Birthday.Value = (DateTime?)settings["user_birthday"];
                male.IsChecked = (bool?)(((int)settings["user_gender"]) == 0);
                female.IsChecked = (bool?)(((int)settings["user_gender"]) == 1);
                location.SelectedIndex = (int)settings["user_location"];
                education.SelectedIndex = (int)settings["user_education"];
                phone.Text = settings["user_phone"].ToString();
                email.Text = settings["user_email"].ToString();

                arabic.IsChecked = (bool?)settings["user_details_arabic"];
                religon.IsChecked = (bool?)settings["user_details_religon"];
                olim.IsChecked = (bool?)settings["user_details_olim"];
                singleParent.IsChecked = (bool?)settings["user_details_singleParent"];
                maahager.IsChecked = (bool?)settings["user_details_maahager"];
                etyopee.IsChecked = (bool?)settings["user_details_etyopee"];
                disabled.IsChecked = (bool?)settings["user_details_disabled"];
                exPrisioner.IsChecked = (bool?)settings["user_details_exPrisioner"];

                familyState.SelectedIndex = (int)settings["user_family_state"];
                certificate.IsChecked = (bool?)settings["user_cetificates"];
                computerKnoledge.SelectedIndex = (int)settings["user_computer_knowledge"];
                driverLicence.IsChecked = (bool?)settings["user_driver_licence"];

                service1.IsChecked = (bool?)settings["user_service_1"];
                service2.IsChecked = (bool?)settings["user_service_2"];
                service3.IsChecked = (bool?)settings["user_service_3"];
                service4.IsChecked = (bool?)settings["user_service_4"];
                service5.IsChecked = (bool?)settings["user_service_5"];
                service6.IsChecked = (bool?)settings["user_service_6"];
                service7.IsChecked = (bool?)settings["user_service_7"];
                service8.IsChecked = (bool?)settings["user_service_8"];
                service9.IsChecked = (bool?)settings["user_service_9"];
                service10.IsChecked = (bool?)settings["user_service_10"];
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("mand"))
            {
                NavigationContext.QueryString.Remove("mand");
                pivotController.SelectedIndex = 1;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (firstName.Text.Length == 0 || lastName.Text.Length == 0)
            {
                MessageBox.Show("חובה למלא את כל השדות!", "שגיאה!", MessageBoxButton.OK);
                pivotController.SelectedIndex = 0;
                return;
            }

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings["user_firstname"] = firstName.Text;
            settings["user_lastname"] = lastName.Text;
            settings["user_birthday"] = Birthday.Value.Value;
            settings["user_gender"] = male.IsChecked.Value ? 0 : 1;
            settings["user_location"] = location.SelectedIndex;
            settings["user_education"] = education.SelectedIndex;
            settings["user_phone"] = phone.Text;
            settings["user_email"] = email.Text;

            settings["user_details_arabic"] = arabic.IsChecked.Value;
            settings["user_details_religon"] = religon.IsChecked.Value;
            settings["user_details_olim"] = olim.IsChecked.Value;
            settings["user_details_singleParent"] = singleParent.IsChecked.Value;
            settings["user_details_maahager"] = maahager.IsChecked.Value;
            settings["user_details_etyopee"] = etyopee.IsChecked.Value;
            settings["user_details_disabled"] = disabled.IsChecked.Value;
            settings["user_details_exPrisioner"] = exPrisioner.IsChecked.Value;

            settings["user_family_state"] = familyState.SelectedIndex;
            settings["user_cetificates"] = certificate.IsChecked.Value;
            settings["user_computer_knowledge"] = computerKnoledge.SelectedIndex;
            settings["user_driver_licence"] = driverLicence.IsChecked.Value;

            settings["user_service_1"] = service1.IsChecked.Value;
            settings["user_service_2"] = service2.IsChecked.Value;
            settings["user_service_3"] = service3.IsChecked.Value;
            settings["user_service_4"] = service4.IsChecked.Value;
            settings["user_service_5"] = service5.IsChecked.Value;
            settings["user_service_6"] = service6.IsChecked.Value;
            settings["user_service_7"] = service7.IsChecked.Value;
            settings["user_service_8"] = service8.IsChecked.Value;
            settings["user_service_9"] = service9.IsChecked.Value;
            settings["user_service_10"] = service10.IsChecked.Value;

            settings.Save();

            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pivotController.SelectedIndex = 0;
        }

        private void familyState_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

    }
}