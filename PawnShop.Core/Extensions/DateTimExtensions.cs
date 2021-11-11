using System;

namespace PawnShop.Core.Extensions
{
    public static class DateTimExtensions
    {
        public static DateTime Yesterday(this DateTime dateTime)
        {
            return dateTime.AddDays(-1);
        }

        public static DateTime Monday(this DateTime dateTime)
        {
            return dateTime.AddDays(-(int)DateTime.Now.DayOfWeek);
        }

        public static DateTime Sunday(this DateTime dateTime)
        {
            var days = DayOfWeek.Saturday - DateTime.Now.DayOfWeek;
            days += 1;
            return dateTime.AddDays(days);
        }

        public static DateTime PastMonday(this DateTime dateTime)
        {
            return dateTime.Monday().AddDays(-7);
        }

        public static DateTime PastSunday(this DateTime dateTime)
        {
            return dateTime.Sunday().AddDays(-7);
        }

        public static DateTime BeginningOfCurrentMonth(this DateTime dateTime)
        {
            return dateTime.AddDays(-(DateTime.Today.Day - 1));
        }

        public static DateTime EndOfCurrentMonth(this DateTime dateTime)
        {
            return dateTime.BeginningOfCurrentMonth()
                .AddDays(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1);
        }

        public static DateTime BeginningOfPastMonth(this DateTime dateTime)
        {
            return dateTime.BeginningOfCurrentMonth().AddMonths(-1);
        }

        public static DateTime EndOfPastMonth(this DateTime dateTime)
        {
            return dateTime.EndOfCurrentMonth().AddMonths(-1);
        }

        public static DateTime BeginningOfCurrentQuarter(this DateTime dateTime)
        {
            return dateTime.BeginningOfCurrentMonth().AddMonths(-2);
        }

        public static DateTime EndOfCurrentQuarter(this DateTime dateTime)
        {
            return dateTime.EndOfCurrentMonth();
        }

        public static DateTime BeginningOfPastQuarter(this DateTime dateTime)
        {
            return dateTime.BeginningOfCurrentQuarter().AddMonths(-3);
        }

        public static DateTime EndOfPastQuarter(this DateTime dateTime)
        {
            return dateTime.EndOfCurrentQuarter().AddMonths(-3);
        }

        public static DateTime BeginningOfCurrentYear(this DateTime dateTime)
        {
            var currentDay = dateTime.Day;
            var currentMonth = dateTime.Month;
            currentDay -= 1;
            currentMonth -= 1;
            return dateTime
                .AddDays(-currentDay)
                .AddMonths(-currentMonth);
        }

        public static DateTime EndOfCurrentYear(this DateTime dateTime)
        {
            var currentYear = dateTime.Year + 1;
            return new DateTime(currentYear, 1, 1).AddDays(-1);
        }

        public static DateTime BeginningOfPastYear(this DateTime dateTime)
        {
            return dateTime.BeginningOfCurrentYear().AddYears(-1);
        }

        public static DateTime EndOfPastYear(this DateTime dateTime)
        {
            return dateTime.EndOfCurrentYear().AddYears(-1);
        }
    }
}