using System;
using System.Globalization;

namespace Transfer.Core.Helpers
{
    public static class DateHelpers
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static string ToPersianDate(this DateTime input, bool withTime = true, string dateSeperator = "/")
        {
            if (input == null) return String.Empty;
            PersianCalendar pCal = new PersianCalendar();
            string returnValue = String.Format("{1:0000}{0}{2:00}{0}{3:00}", dateSeperator, pCal.GetYear(input), pCal.GetMonth(input), pCal.GetDayOfMonth(input));
            if (withTime) returnValue += String.Format(" {0:00}:{1:00}:{2:00}", input.Hour, input.Minute,input.Second);
            return returnValue;
        }
        public static string ToPersianDate(this DateTime? input,bool withTime = true,string dateSeperator = "/")
        {
            if (!input.HasValue) return String.Empty;
            return input.Value.ToPersianDate(withTime, dateSeperator);
        }

        public static DateTime? ToDateTime(this string input, bool withTime = true, string dateSeperator = "/")
        {
            if (input == null) return null;

            string[] inputValues;
            if (withTime)
                inputValues = input.Split(" ");
            else
                inputValues = input.Split(dateSeperator);

            if (withTime && inputValues.Length != 2) return null;
            if (!withTime && inputValues.Length != 3) return null;

            if (withTime)
            {
                string[] dateValues = inputValues[0].Split(dateSeperator);
                string[] timeValues = inputValues[1].Split(":");

                if (dateValues.Length != 3) return null;
                if (timeValues.Length != 3) return null;

                if (int.TryParse(dateValues[0], out int year) &&
                int.TryParse(dateValues[1], out int month) &&
                int.TryParse(dateValues[2], out int day) &&
                int.TryParse(timeValues[0], out int hour) &&
                int.TryParse(timeValues[1], out int minute)&&
                int.TryParse(timeValues[2], out int second))
                {
                    PersianCalendar pCal = new PersianCalendar();
                    return pCal.ToDateTime(year, month, day, hour, minute, second, 0);
                }
                else return null;
            }
            else
            {                
                if (int.TryParse(inputValues[0], out int year) &&
                int.TryParse(inputValues[1], out int month) &&
                int.TryParse(inputValues[2], out int day))
                {
                    PersianCalendar pCal = new PersianCalendar();
                    return pCal.ToDateTime(year, month, day, 0, 0, 0, 0);
                }
                else return null;
            }                                              
        }

        public static long DateTimeToTimestamp(DateTime value)
        {            
            return (long)Math.Floor((value.ToUniversalTime() - UnixEpoch).TotalMilliseconds);                       
        }

        public static DateTime TimeStampToDateTime(long? millis)
        {           
            return UnixEpoch.AddMilliseconds(Convert.ToDouble(millis)).ToLocalTime();
        }       
    }
}
