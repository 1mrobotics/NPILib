using System;

namespace NPILib
{
    public class Hour
    {
        // Properties
        public int[] HourValue { get; private set; }    // Array of 2 integers for the hour
        public int[] Minutes { get; private set; }     // Array of 2 integers for the minutes
        public int[] Seconds { get; private set; }     // Array of 2 integers for the seconds (default to 00 if null)

        // Constructors
        public Hour(int hour, int minutes, int? seconds = null)
        {
            HourValue = new int[2];
            Minutes = new int[2];
            Seconds = new int[2];

            SetHour(hour);
            SetMinutes(minutes);
            SetSeconds(seconds ?? 0); // Default to 00 if seconds are null
        }

        // Overloaded constructor (without seconds, defaults seconds to 00)
        public Hour(int hour, int minutes) : this(hour, minutes, 0) { }

        // Overloaded constructor (from string in "hh:mm:ss" or "hh:mm" format)
        public Hour(string time)
        {
            if (string.IsNullOrWhiteSpace(time))
                throw new ArgumentException("Time cannot be null or empty.");

            var parts = time.Split(':');
            if (parts.Length < 2 || parts.Length > 3)
                throw new ArgumentException("Time must be in 'hh:mm' or 'hh:mm:ss' format.");

            HourValue = new int[2];
            Minutes = new int[2];
            Seconds = new int[2];

            SetHour(int.Parse(parts[0]));
            SetMinutes(int.Parse(parts[1]));
            SetSeconds(parts.Length == 3 ? int.Parse(parts[2]) : 0); // Default to 00 if seconds are missing
        }

        // Method to set the Hour array
        public void SetHour(int hour)
        {
            if (hour < 0 || hour > 23)
                throw new ArgumentOutOfRangeException(nameof(hour), "Hour must be between 0 and 23.");

            var hourString = hour.ToString("D2");
            HourValue[0] = int.Parse(hourString[0].ToString());
            HourValue[1] = int.Parse(hourString[1].ToString());
        }

        // Method to set the Minutes array
        public void SetMinutes(int minutes)
        {
            if (minutes < 0 || minutes > 59)
                throw new ArgumentOutOfRangeException(nameof(minutes), "Minutes must be between 0 and 59.");

            var minutesString = minutes.ToString("D2");
            Minutes[0] = int.Parse(minutesString[0].ToString());
            Minutes[1] = int.Parse(minutesString[1].ToString());
        }

        // Method to set the Seconds array
        public void SetSeconds(int seconds)
        {
            if (seconds < 0 || seconds > 59)
                throw new ArgumentOutOfRangeException(nameof(seconds), "Seconds must be between 0 and 59.");

            var secondsString = seconds.ToString("D2");
            Seconds[0] = int.Parse(secondsString[0].ToString());
            Seconds[1] = int.Parse(secondsString[1].ToString());
        }

        // Method to get the full time as a string (default format is "hh:mm:ss")
        public string GetFullTime()
        {
            var hour = $"{HourValue[0]}{HourValue[1]}";
            var minute = $"{Minutes[0]}{Minutes[1]}";
            var second = $"{Seconds[0]}{Seconds[1]}";

            return $"{hour}:{minute}:{second}";
        }

        // Override ToString for debugging purposes
        public override string ToString()
        {
            return GetFullTime();
        }
    }
}
