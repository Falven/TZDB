/* (c) Copyright Francisco Aguilera (Falven)
 * You are free to edit and distribute this
 * source so long as this statement remains
 * in place, here and in all other such files.
 */

using System;
using System.Data;
using System.Data.Linq;

namespace NUIClockUpdater.Models
{
    partial class Leap
    {
        public const string TABLE_SCHEMA = "dbo";
        public const string TABLE_NAME = "leaps";
        public const string FULL_TABLE_NAME = TABLE_SCHEMA + "." + TABLE_NAME;

        public const string LEAP_NAME = "Leap";
        public const int YearIndex = 0;
        public const int MonthIndex = 1;
        public const int DayIndex = 2;
        public const int TimeIndex = 3;
        public const int CorrectionIndex = 4;
        public const int RsIndex = 5;

        public static short ParseYear(string y)
        {
            short year = 0;
            if (!short.TryParse(y, out year))
            {
                App.ExceptionLogger.LogLow(new Exception("LeapOps.cs Ln 26: Error while parsing the Year field for a Leap entry."));
            }
            return year;
        }

        public static short ParseDay(string d)
        {
            short day;
            if (!short.TryParse(d, out day))
            {
                App.ExceptionLogger.LogLow(new Exception("LeapOps.cs ln 41: Error while parsing the Day field for a Leap entry."));
            }
            return day;
        }

        public static TimeSpan ParseTime(string t)
        {
            TimeSpan time;
            if (!TimeSpan.TryParse(t, out time))
            {
                App.ExceptionLogger.LogLow(new Exception("LeapOps.cs Ln 51: Error while parsing the Time field for a Leap entry."));
            }
            return time;
        }

        public static char ParseCorrection(string c)
        {
            char correction;
            if (!char.TryParse(c, out correction))
            {
                App.ExceptionLogger.LogLow(new Exception("LeapOps.cs Ln 61: Error while parsing the Correction field for a Leap entry."));
            }
            return correction;
        }

        public static char ParseRs(string r)
        {
            char rs;
            if (!char.TryParse(r, out rs))
            {
                App.ExceptionLogger.LogLow(new Exception("LeapOps.cs Ln 71: Error while parsing the RS field for a Leap entry."));
            }
            return rs;
        }
    }
}
