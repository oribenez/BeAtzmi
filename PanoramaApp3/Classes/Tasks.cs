using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes
{
    [Table("Tasks")]
    class Tasks : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string text { get; set; }
        public DateTime dueDate { get; set; }
        public bool done
        {
            get { return _done; }
            set
            {
                _done = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("done"));
                }
            }
        }
        public string formattedString
        {
            get
            {
                return String.Format("{0:dd/MM/yy HH:mm}", dueDate);
            }
        }

        private bool _done;

        public override string ToString()
        {
            return ID.ToString() + ": " + text;
        }
    }
}
