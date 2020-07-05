using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;

namespace BeAtzmi
{
    public partial class Contact : PhoneApplicationPage
    {
        public Contact()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            phone.Text = settings["user_phone"].ToString();
            email.Text = settings["user_email"].ToString();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            EmailComposeTask mailMessage = new EmailComposeTask();

            string DateX = DateTime.Now.ToString("dd/MM/yyyy");
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            string name = String.Format("{0} {1}", settings["user_firstname"].ToString(), settings["user_lastname"].ToString());
            string SubjectX = "פנייה לתעסו-קו בתאריך: " + DateX + " (" + name + ")";

            string content = String.Format("Name: {0}\nDate: {1}\nPhone: {2}\nEmail: {3}\nSubject: {5}\nContent:\n{4}\n\n\n\nSent by Be-Atzmi App.", name, DateX, phone.Text, email.Text, contentTB.Text, subject.Text);

            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = SubjectX;
            emailComposeTask.Body = content;
            emailComposeTask.To = "tkav@be-atzmi.org.il";

            emailComposeTask.Show();

        }

    }
}