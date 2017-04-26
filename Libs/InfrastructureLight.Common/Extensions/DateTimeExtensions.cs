using System;

namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the <see cref="System.DateTime"/>
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>        
        ///     Indicates the specified <see cref="System.DateTime"/> is between a DateRange
        ///     or <see cref="System.DateTime"/> is null.
        /// </summary>
        public static bool IsNullOrOutOfRange(this DateTime? source, DateTime startDate, DateTime endDate)
        {
            return source.HasValue
               ? IsOutOfRange(source.Value, startDate, endDate)
               : source.IsNull();
        }

        /// <summary>        
        ///     Indicates the specified <see cref="System.DateTime"/> is between a DateRange
        /// </summary>   
        public static bool IsOutOfRange(this DateTime source, DateTime startDate, DateTime endDate)
        {
            return source < startDate || source > endDate;
        }

        /// <summary>
        ///     Calculating total night time        
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/questions/14123679/calculating-total-night-time-from-timespan"/>
        public static double CalcNightDuration(DateTime start, DateTime end, int startHour, int startMin, int endHour, int endMin)
        {
            if (start > end) { throw new Exception(); }

            TimeSpan nightStart = new TimeSpan(startHour, startMin, 0);
            TimeSpan nightEnd = new TimeSpan(endHour, endMin, 0);

            // check to see if any overlapping 
            // actually happens:
            if (start.Date == end.Date && start.TimeOfDay >= nightEnd && end.TimeOfDay <= nightStart)
            {
                // no overlapping occurs 
                // so return 0:
                return 0;
            }

            // check if same day as will process 
            // this differently:
            if (start.Date == end.Date)
            {
                if (start.TimeOfDay > nightStart || end.TimeOfDay < nightEnd)
                {
                    return (end - start).TotalHours;
                }

                double total = 0;
                if (start.TimeOfDay < nightEnd)
                {
                    total += (nightEnd - start.TimeOfDay).TotalHours;
                }
                if (end.TimeOfDay > nightStart)
                {
                    total += (end.TimeOfDay - nightStart).TotalHours;
                }
                return total;
            }
            else // spans multiple days
            {
                double total = 0;

                // add up first day
                if (start.TimeOfDay < nightEnd)
                {
                    total += (nightEnd - start.TimeOfDay).TotalHours;
                }
                if (start.TimeOfDay < nightStart)
                {
                    total += ((new TimeSpan(24, 0, 0)) - nightStart).TotalHours;
                }
                else
                {
                    total += ((new TimeSpan(24, 0, 0)) - start.TimeOfDay).TotalHours;
                }

                // add up the last day
                if (end.TimeOfDay > nightStart)
                {
                    total += (end.TimeOfDay - nightStart).TotalHours;
                }
                if (end.TimeOfDay > nightEnd)
                {
                    total += nightEnd.TotalHours;
                }
                else
                {
                    total += end.TimeOfDay.TotalHours;
                }

                // add up any full days
                int numberOfFullDays = (end - start).Days;
                if (end.TimeOfDay > start.TimeOfDay)
                {
                    numberOfFullDays--;
                }
                if (numberOfFullDays > 0)
                {
                    double hoursInFullDay = ((new TimeSpan(24, 0, 0)) - nightStart).TotalHours + nightEnd.TotalHours;
                    total += hoursInFullDay * numberOfFullDays;
                }

                return total;
            }
        }
    }
}
