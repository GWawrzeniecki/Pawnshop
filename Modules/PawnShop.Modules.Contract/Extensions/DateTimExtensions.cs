using System;

namespace PawnShop.Modules.Contract.Extensions
{
    public static class DateTimExtensions
    {
        public static DateTime Yesterday(this DateTime dateTime) => dateTime.AddDays(-1);

        public static DateTime Monday(this DateTime dateTime) => dateTime.AddDays(-(int)DateTime.Now.DayOfWeek);

        public static DateTime Sunday(this DateTime dateTime)
        {
            var days = DayOfWeek.Saturday - DateTime.Now.DayOfWeek;
            days += 1;
            return DateTime.Now.AddDays(days);
        }
    }
}