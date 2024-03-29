﻿namespace WeeklyPay.Extensions;
internal static class DateExtensions
{
    public static bool IsBankingWeekend(this DateTime date)
    {
        DayOfWeek dayName = date.DayOfWeek;
        bool isSunday = dayName == DayOfWeek.Sunday;
        bool isMonday = dayName == DayOfWeek.Monday;

        return (isSunday || isMonday);
    }

    /// <summary>
    /// Determines if this date is a federal holiday.
    /// </summary>
    /// <param name="date">This date</param>
    /// <returns>True if this date is a federal holiday</returns>
    public static bool IsFederalHoliday(this DateTime date)
    {
        // To ease typing
        int nthWeekDay = (int)(Math.Ceiling((double)date.Day / 7.0d));
        DayOfWeek dayName = date.DayOfWeek;
        bool isThursday = dayName == DayOfWeek.Thursday;
        bool isFriday = dayName == DayOfWeek.Friday;
        bool isMonday = dayName == DayOfWeek.Monday;
        bool isWeekend = dayName == DayOfWeek.Saturday || dayName == DayOfWeek.Sunday;

        // New Year's Day (Jan 1, or preceding Friday/following Monday if weekend)
        if ((date.Month == 12 && date.Day == 31 && isFriday) ||
            (date.Month == 1 && date.Day == 1 && !isWeekend) ||
            (date.Month == 1 && date.Day == 2 && isMonday))
            return true;

        // MLK day (3rd Monday in January)
        if (date.Month == 1 && isMonday && nthWeekDay == 3)
            return true;

        // President’s Day (3rd Monday in February)
        if (date.Month == 2 && isMonday && nthWeekDay == 3)
            return true;

        // Memorial Day (Last Monday in May)
        if (date.Month == 5 && isMonday && date.AddDays(7).Month == 6)
            return true;

        // Independence Day (July 4, or preceding Friday/following Monday if weekend)
        if ((date.Month == 7 && date.Day == 3 && isFriday) ||
            (date.Month == 7 && date.Day == 4 && !isWeekend) ||
            (date.Month == 7 && date.Day == 5 && isMonday))
            return true;

        // Labor Day (1st Monday in September)
        if (date.Month == 9 && isMonday && nthWeekDay == 1)
            return true;

        // Columbus Day (2nd Monday in October)
        if (date.Month == 10 && isMonday && nthWeekDay == 2)
            return true;

        // Veterans Day (November 11, or preceding Friday/following Monday if weekend)
        if ((date.Month == 11 && date.Day == 10 && isFriday) ||
            (date.Month == 11 && date.Day == 11 && !isWeekend) ||
            (date.Month == 11 && date.Day == 12 && isMonday))
            return true;

        // Thanksgiving (4th Thursday in November)
        if (date.Month == 11 && isThursday && nthWeekDay == 4)
            return true;

        // Christmas (December 25)
        if (date.Month == 12 && date.Day == 25)
            return true;

        return false;
    }
}
