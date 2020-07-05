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
using System.IO.IsolatedStorage;

namespace BeAtzmi.Pages
{
    public partial class Knowledge : PhoneApplicationPage
    {
        public Knowledge()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //update the status of step 3
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            int newGrade = 70, oldGrade = (int)settings["step3Percentage"];
            if (newGrade > oldGrade)
            {
                settings["step3Percentage"] = newGrade;
            } 
            settings["bubbleText"] = "יפה! כבר נכנסת לשם,\nאז למה שלא תמצא שירות שיעזור לך?";


            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            var query = dbConn.Table<Service>();
            List<Service> allServices = await query.ToListAsync();
            List<Service> selectedServices = new List<Service>();

            foreach (Service service in allServices)
            {
                //Check if service should be selected by any of the parameter
                if (isCompatible(service))
                {
                    //TRUE: add to selectedService; remove from allServices;
                    selectedServices.Add(service);
                }
                //ELSE: do nothing;
            }

            servicesList.DataContext = selectedServices;
        }

        private bool isCompatible(Service service)
        {
            if (service.isDefault) return true;

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            Service.AgeCode ageCode = GetAgeCode(((DateTime)settings["user_birthday"]));
            Service.GenderCode genderCode = getGenderCode(((int)settings["user_gender"]));

            bool arab = (bool)settings["user_details_arabic"];
            bool religion = (bool)settings["user_details_religon"];
            bool olim = (bool)settings["user_details_olim"];
            bool singleParent = (bool)settings["user_details_singleParent"];
            bool workImmigrations = (bool)settings["user_details_maahager"];
            bool etyopee = (bool)settings["user_details_etyopee"];
            bool disabled = (bool)settings["user_details_disabled"];
            bool exPrisionerNAddicted = (bool)settings["user_details_exPrisioner"];

            bool workersRightNleagalAssistence = (bool)settings["user_service_1"];
            bool baseStudy = (bool)settings["user_service_2"];
            bool proStudy = (bool)settings["user_service_3"];
            bool milgot = (bool)settings["user_service_4"];
            bool buissnesInitiatives = (bool)settings["user_service_5"];
            bool houseBudgetManage = (bool)settings["user_service_6"];
            bool GOVemploy = (bool)settings["user_service_8"];
            bool ORGemploy = (bool)settings["user_service_9"];
            bool israelEmployPlan = (bool)settings["user_service_10"];

            if (service.ageCode != Service.AgeCode.All && service.ageCode != ageCode) return false;
            if (service.genderCode != Service.GenderCode.Both && service.genderCode != genderCode) return false;

            if (arab && service.arab) return true;
            if (religion && service.religion) return true;
            if (olim && service.olim) return true;
            if (singleParent && service.singleParent) return true;
            if (workImmigrations && service.workImmigrations) return true;
            if (etyopee && service.etyopee) return true;
            if (disabled && service.disabled) return true;
            if (exPrisionerNAddicted && service.exPrisionerNAddicted) return true;
            if (workersRightNleagalAssistence && service.workersRightNleagalAssistence) return true;
            if (baseStudy && service.baseStudy) return true;
            if (proStudy && service.proStudy) return true;
            if (milgot && service.milgot) return true;
            if (buissnesInitiatives && service.buissnesInitiatives) return true;
            if (houseBudgetManage && service.houseBudgetManage) return true;
            if (GOVemploy && service.GOVemploy) return true;
            if (ORGemploy && service.ORGemploy) return true;
            if (israelEmployPlan && service.israelEmployPlan) return true;

            if (genderCode == Service.GenderCode.FemaleOnly)
            {
                if (service.genderCode == Service.GenderCode.FemaleOnly)
                {
                    return true;
                }
            }

            return false;
        }

        private Service.GenderCode getGenderCode(int p)
        {
            return (p == 0) ? Service.GenderCode.MaleOnly : Service.GenderCode.FemaleOnly;
        }

        private Service.AgeCode GetAgeCode(DateTime dateTime)
        {
            int age = DateTime.Now.Year - dateTime.Year;

            if (age >= 45)
            {
                return Service.AgeCode.FourtyFivePlus;
            }
            else if (age >= 18 && age <= 35)
            {
                return Service.AgeCode.EightteenTillThirtyFive;
            }
            else
            {
                return Service.AgeCode.All;
            }
        }

        private void servicesList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            int id = (((LongListSelector)sender).SelectedItem as Service).ID;
            NavigationService.Navigate(new Uri("/Pages/SingleService.xaml?serviceID=" + id.ToString(), UriKind.Relative));
        }
    }
}