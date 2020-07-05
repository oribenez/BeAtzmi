using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Live;
using Microsoft.Live.Controls;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Phone.Tasks;

namespace BeAtzmi.Pages.ResumeBuilderPages
{
    public partial class PublishFile : PhoneApplicationPage
    {
        private LiveConnectClient client;

        public PublishFile()
        {
            InitializeComponent();
            shareButton.Visibility = System.Windows.Visibility.Collapsed;
            end.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void skydrive_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                this.client = new LiveConnectClient(e.Session);
                this.GetAccountInformations();
            }
            else
            {
                this.client = null;
                InfoText.Text = e.Error != null ? e.Error.ToString() : string.Empty;
            }
        }

        private async void GetAccountInformations()
        {
            try
            {
                LiveOperationResult operationResult = await this.client.GetAsync("me");
                signInButton.Visibility = System.Windows.Visibility.Collapsed;
                var jsonResult = operationResult.Result as dynamic;
                string firstName = jsonResult.first_name ?? string.Empty;
                string lastName = jsonResult.last_name ?? string.Empty;
                InfoText.Text = "שלום לך " + firstName + " " + lastName;

                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var fileStream = store.OpenFile(NavigationContext.QueryString["filePath"], FileMode.Open))
                    {
                        try
                        {
                            InfoText.Text = "מעלה את קורות החיים ל-SkyDrive שלך...";

                            LiveOperationResult res = await client.UploadAsync("me/skydrive", "קורות חיים.docx", fileStream.AsInputStream().AsStreamForRead(), OverwriteOption.Overwrite);

                            InfoText.Text = "הקובץ " + NavigationContext.QueryString["filePath"] + " הועלה בהצלחה ל-SkyDrive שלך!";

                            shareButton.Visibility = System.Windows.Visibility.Visible;
                            end.Visibility = System.Windows.Visibility.Visible;
                            shareButton.Tag = res.Result["source"];

                            prog.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                InfoText.Text = e.ToString();
            }
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("הקובץ הועלה ל-SkyDrive שלך!\nהאם ברצונך לשלוח את הקישור להורדת הקובץ במייל?", "העלאה הסתיימה בהצלחה", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                EmailComposeTask emailComposeTask = new EmailComposeTask();

                emailComposeTask.Subject = "קו\"ח שנוצרו בעזרת אפליקציית בעצמי";
                emailComposeTask.Body = "קישור להורדת הקובץ:\n";
                emailComposeTask.Body += ((Button)sender).Tag.ToString();

                emailComposeTask.Show();
            }
        }

        private void end_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            prog.Visibility = System.Windows.Visibility.Visible;
        }
    }
}