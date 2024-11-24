using System;

namespace NPILib
{
    public class Time
    {
        // Properties
        public Date Date { get; private set; }    // Date object
        public Hour Hour { get; private set; }    // Hour object

        // Constructor
        public Time(Date date, Hour hour)
        {
            Date = date ?? throw new ArgumentNullException(nameof(date), "Date cannot be null.");
            Hour = hour ?? throw new ArgumentNullException(nameof(hour), "Hour cannot be null.");
        }

        // Constructor from individual date and time components
        public Time(int day, int month, int year, int hour, int minutes, int? seconds = null)
        {
            Date = new Date(day, month, year);
            Hour = new Hour(hour, minutes, seconds);
        }

        // Constructor from strings for date and time
        public Time(string date, string time)
        {
            if (string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(time))
                throw new ArgumentException("Date and time cannot be null or empty.");

            // Assuming date is in "dd/mm/yyyy" and time is in "hh:mm:ss" or "hh:mm" format
            var dateParts = date.Split('/');
            if (dateParts.Length != 3)
                throw new ArgumentException("Date must be in 'dd/mm/yyyy' format.");

            var day = int.Parse(dateParts[0]);
            var month = int.Parse(dateParts[1]);
            var year = int.Parse(dateParts[2]);

            Date = new Date(day, month, year);
            Hour = new Hour(time);
        }

        // Constructor from DateTime object
        public Time(DateTime dateTime)
        {
            // Break down DateTime into Date and Hour components
            Date = new Date(dateTime.Day, dateTime.Month, dateTime.Year);
            Hour = new Hour(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        // Method to get the full timestamp as a string
        public string GetFullTimestamp(string dateFormat = "dd/mm/yyyy")
        {
            return $"{Date.GetFullDate(dateFormat)} {Hour.GetFullTime()}";
        }

        // Override ToString for debugging purposes
        public override string ToString()
        {
            return GetFullTimestamp();
        }
    }
}
