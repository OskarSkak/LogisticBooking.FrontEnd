using System;

namespace LogisticsBooking.FrontEnd.ConfigHelpers
{
    public static class TimeUtility
    {

        public static bool IsWithin(TimeSpan containerStart , TimeSpan containerEnd , TimeSpan toBeCheckedInStart ,TimeSpan toBeCheckedInEnd)
        {
            return Check(containerStart, containerEnd, toBeCheckedInStart) &&
                   Check(containerStart, containerEnd, toBeCheckedInEnd);

        }

        public static bool IsOverlapping(TimeSpan containerStart, TimeSpan containerEnd, TimeSpan toBeCheckedInStart,
            TimeSpan toBeCheckedInEnd)
        {
            return Check(containerStart, containerEnd, toBeCheckedInStart) ||
                   Check(containerStart, containerEnd, toBeCheckedInEnd);
        }
        
        private static bool Check(TimeSpan start, TimeSpan end, TimeSpan now)
        {
            if (start <= end)
            {
                // start and stop times are in the same day
                if (now >= start && now <= end)
                {
                    return true;
                }
            }
            else
            {
                // start and stop times are in different days
                if (now >= start || now <= end)
                {
                    return true;
                }
            }

            return false;
        }
    }
}