using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace BeAtzmi.Classes
{
    public class InterviewsAppointmentSource : AppointmentSource
    {
        //public InterviewsAppointmentSource() { }

        
        public async override void FetchData(DateTime startDate, DateTime endDate)
        {
            this.AllAppointments.Clear();

            SQLiteAsyncConnection dbConn = new SQLiteAsyncConnection(App.DB_PATH);
            List<Interviews> appo = await dbConn.Table<Interviews>().ToListAsync();

            foreach (Interviews interview in appo)
            {
                int a = interview.Datetime.CompareTo(startDate);
                int b = interview.Datetime.CompareTo(endDate);
                if (a >= 0 && b <= 0)
                {
                    this.AllAppointments.Add(new InterviewAppointment()
                    {
                        StartDate = interview.Datetime,
                        EndDate = interview.Datetime.AddMinutes(45),
                        Subject = "ראיון עבודה - " + interview.CompanyName,
                        Location = interview.Address,
                        InterviewData = interview,
                        Details = interview.ContactName + " - " + interview.ContactPhone
                    });
                }
            }

            this.OnDataLoaded();
        }
    }

    public class InterviewAppointment : IAppointment
    {
        /// <summary>
        /// Gets the subject of the appointment.
        /// </summary>
        public string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the start date and time of the appointment.
        /// </summary>
        public DateTime StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the end date and time of the appointment.
        /// </summary>
        public DateTime EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the details of the appointment.
        /// </summary>
        public string Details
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the location of the appointment.
        /// </summary>
        public string Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a string representation of the appointment
        /// </summary>
        /// <returns>A string representation of the appointment</returns>
        public override string ToString()
        {
            return this.Subject;
        }

        public Interviews InterviewData { get; set; }
    }
}
