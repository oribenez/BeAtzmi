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
    public partial class AddLanguage : PhoneApplicationPage
    {
        public AddLanguage()
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
            if (langName.Text.Replace(" ", "").Length == 0)
            {
                MessageBox.Show("חובה להזין מידע בכל אחד מן השדות!", "תקלה!", MessageBoxButton.OK);
                return;
            }
            PhoneApplicationService.Current.State["resumeItem"] = new ResumeLanguage(langName.Text, (LanguageKnowledgeLevel)level.SelectedIndex);
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }
    }
}