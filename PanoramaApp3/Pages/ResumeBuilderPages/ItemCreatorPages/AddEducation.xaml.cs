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
    public partial class AddEducation : PhoneApplicationPage
    {
        public AddEducation()
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
            string com = (comments.Text.Replace(" ", "").Length > 0) ? comments.Text : "";
            if (inst.Text.Length > 0 && certificateTB.Text.Length > 0 && special.Text.Length > 0)
            {
                PhoneApplicationService.Current.State["resumeItem"] = new ResumeEducation(yearStart.Value.Value.Year, (today.IsChecked.Value ? 0 : yearEnd.Value.Value.Year), inst.Text, certificateTB.Text, special.Text, com);
                NavigationService.GoBack();
                NavigationService.RemoveBackEntry();
            }
            else
            {
                MessageBox.Show("חובה להזין מידע בכל אחד מן השדות!", "תקלה!", MessageBoxButton.OK);
            }
        }
    }
}