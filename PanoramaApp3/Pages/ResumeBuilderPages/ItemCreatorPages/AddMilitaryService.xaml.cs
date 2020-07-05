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

namespace BeAtzmi.Pages.ResumeBuilderPages.ItemCreatorPages
{
    public partial class AddMilitaryService : PhoneApplicationPage
    {
        public AddMilitaryService()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            PhoneApplicationService.Current.State["resumeItem"] = null;
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (job.Text.Replace(" ", "").Length == 0 || jobDetail.Text.Replace(" ", "").Length == 0 || place.Text.Replace(" ", "").Length == 0)
            {
                MessageBox.Show("חובה להזין מידע בכל אחד מן השדות!", "תקלה!", MessageBoxButton.OK);
                return;
            }

            PhoneApplicationService.Current.State["resumeItem"] = new ResumeMilitaryService(job.Text, jobDetail.Text, yearStart.Value.Value.Year, (today.IsChecked.Value ? 0 : yearEnd.Value.Value.Year), place.Text);
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }
    }
}