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
using BeAtzmi.Classes;
using SQLite;
using System.IO;

namespace BeAtzmi
{
    public partial class goodJob1 : PhoneApplicationPage
    {
        public goodJob1()
        {
            InitializeComponent();
        }

        int _total;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {


            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            _total = int.Parse(NavigationContext.QueryString["total"]);
            object p = NavigationContext.QueryString["p"];
            string answers = (string)p;
            answers = answers.Substring(0, answers.Length - 1);
            List<object> wrong = new List<object>();
            int rightAnswers = 0;

            string[] quests = answers.Split(';');
            for (int i = 0; i < quests.Length; i++)
            {
                string[] data = quests[i].Split('-');
                if (data[1] == "1") rightAnswers++;
                else
                {
                    var query = dbConn.Table<Questions>();
                    int wrongID = int.Parse(data[0]);
                    Questions w = await query.Where((Questions q) => q.ID == wrongID).FirstOrDefaultAsync();

                    string selected, one, two;

                    if (int.Parse(data[1]) - 1 == 1)
                    {
                        selected = w.WrongAnswer1;
                        one = w.WrongAnswer2;
                        two = w.WrongAnswer3;
                    }
                    else if (int.Parse(data[1]) - 1 == 1)
                    {
                        one = w.WrongAnswer1;
                        selected = w.WrongAnswer2;
                        two = w.WrongAnswer3;
                    }
                    else
                    {
                        one = w.WrongAnswer1;
                        two = w.WrongAnswer2;
                        selected = w.WrongAnswer3;
                    }

                    var wrongQuestion = new
                    {
                        question = w.Question,
                        right = w.RightAnswer,
                        select = selected,
                        wrong1 = one,
                        wrong2 = two
                    };

                    wrong.Add(wrongQuestion);
                }
            }

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            int newGrade = 26, oldGrade = (int)settings["step2Percentage"];

            string fileName = "feedback";

            if (rightAnswers == _total)
            {
                fileName += "4";
                settings["bubbleText"] = "אתה ממש מעולה\nברעיונות עבודה!";
                newGrade = 100;
            }
            else if (rightAnswers >= 7)
            {
                fileName += "3";
                settings["bubbleText"] = "אתה ממש מעולה\nברעיונות עבודה!";
                newGrade = 100;
            }
            else if (rightAnswers >= 4)
            {
                fileName += "2";
                settings["bubbleText"] = "אתה טוב ברעיונות עבודה\nאבל נראלי שאתה יכול יותר...";
                newGrade = 67;
            }
            else
            {
                fileName += "1";
                settings["bubbleText"] = "וואלה, נראלי שכדאי שתנסה שוב\nאת השאלות לפני הראיון...";
                newGrade = 26;
            }

            fileName += ".txt";

            if (newGrade > oldGrade) settings["step2Percentage"] = newGrade;

            settings.Save();

            string content; //read data from fileName

            TextReader input = new StreamReader(Application.GetResourceStream(new Uri("Assets/Feedbacks/" + fileName, UriKind.Relative)).Stream);
            content = input.ReadToEnd();

            content = String.Format(content, rightAnswers, _total);

            feedback.Text = content;

            wrongQuestionListBox.DataContext = wrong;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Knowledge.xaml", UriKind.Relative));
        }
    }
}