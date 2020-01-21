using System;

namespace LogisticsBooking.FrontEnd.DataServices.Utilities
{
    public class GeneralUtil
    {
        /**
         * Returns a DateTime Object with specified time.
         * Everything but hour and minute is min value.
         */
        public static DateTime TimeFromHourAndMinute(int hour, int minute)
        {
           int year = 0001;
            int month = 01;
            int day = 01;
            int second = 00;
            int millisecond = 0;
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        
        /**
         * BASIC INPUT IN TWO FIELDS:
         *Full:
         * <form method="post">
                <input type="number" value="@Model.date.Hour" name="ViewHour" style="width: 2.5em" max="23" min="0" required name="@REPLACE_TO_GET" /> 
                : <input type="number" value="@Model.date.Minute" name="ViewMinute" style="width: 2.5em" max="59" min="0" required name="@REPLACE_TO_GET" />
                <input type="submit" value="Post" />
            </form>
            
            Partial:
            <input type="number" value="@Model.date.Hour" name="ViewHour" style="width: 2.5em" max="23" min="0" required /> 
            : <input type="number" value="@Model.date.Minute" name="ViewMinute" style="width: 2.5em" max="59" min="0" required />
         */
    }
}