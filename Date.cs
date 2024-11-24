using System;

namespace NPILib
{
    public class Date
    {
        // Properties
        public int[] Day { get; private set; }   // Array of 2 integers for the day
        public int[] Month { get; private set; } // Array of 2 integers for the month
        public int[] Year { get; private set; }  // Array of 4 integers for the year

        // Default date format
        private string DefaultDateFormat { get; set; } = "dd/mm/yyyy";

        // Constructor
        public Date(int day, int month, int year)
        {
            // Initialize arrays
            Day = new int[2];
            Month = new int[2];
            Year = new int[4];

            // Set values
            SetDay(day);
            SetMonth(month);
            SetYear(year);
        }

        // Method to set the Day array
        public void SetDay(int day)
        {
            if (day < 1 || day > 31)
                throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 31.");

            var dayString = day.ToString("D2"); // Ensure 2 digits
            Day[0] = int.Parse(dayString[0].ToString());
            Day[1] = int.Parse(dayString[1].ToString());
        }

        // Method to set the Month array
        public void SetMonth(int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");

            var monthString = month.ToString("D2"); // Ensure 2 digits
            Month[0] = int.Parse(monthString[0].ToString());
            Month[1] = int.Parse(monthString[1].ToString());
        }

        // Method to set the Year array
        public void SetYear(int year)
        {
            if (year < 0 || year > 9999)
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 0 and 9999.");

            var yearString = year.ToString("D4"); // Ensure 4 digits
            for (int i = 0; i < 4; i++)
            {
                Year[i] = int.Parse(yearString[i].ToString());
            }
        }

        // Method to get the full date in a desired format
        public string GetFullDate(string format)
        {
            // Validate input format
            if (string.IsNullOrWhiteSpace(format) ||
                (format != "dd/mm/yyyy" && format != "mm/dd/yyyy"))
            {
                throw new ArgumentException("Invalid date format. Use 'dd/mm/yyyy' or 'mm/dd/yyyy'.");
            }

            // Construct date parts
            string day = $"{Day[0]}{Day[1]}";
            string month = $"{Month[0]}{Month[1]}";
            string year = $"{Year[0]}{Year[1]}{Year[2]}{Year[3]}";

            // Return the formatted date
            return format == "dd/mm/yyyy"
                ? $"{day}/{month}/{year}"
                : $"{month}/{day}/{year}";
        }

        // Method to get the full date using the default format
        public string GetFullDate()
        {
            return GetFullDate(DefaultDateFormat);
        }

        // Override ToString for easier debugging
        public override string ToString()
        {
            return GetFullDate();
        }

        // Method to update the default date format
        public void SetDefaultDateFormat(string format)
        {
            if (string.IsNullOrWhiteSpace(format) ||
                (format != "dd/mm/yyyy" && format != "mm/dd/yyyy"))
            {
                throw new ArgumentException("Invalid date format. Use 'dd/mm/yyyy' or 'mm/dd/yyyy'.");
            }

            DefaultDateFormat = format;
        }
    }
}
