using System;
using System.Text;
using System.Globalization;

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

        /// <summary>
        ///     Converts a date to a day
        /// </summary>
        public static string ToDayString(this DateTime value)
        {
            var sb = new StringBuilder();

            string[] days = { "Вс ", "Пн ", "Вт ", "Ср ", "Чт ", "Пт ", "Сб " };
            if (value != DateTime.MinValue)
            {
                int a = (14 - value.Month) / 12,
                    y = value.Year - a,
                    m = value.Month + 12 * a - 2;

                string day = days[(7000 + (value.Day + y + y / 4 - y / 100 + y / 400 + (31 * m) / 12)) % 7];

                sb.AppendFormat("{0}{1}", day, value.ToShortDateString());
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Converts a date to a month
        /// </summary>
        public static string ToMonthString(this DateTime value, bool isGenitive = false)
        {
            var sb = new StringBuilder();

            string[] month = new string[]
                    {
                        "январь ", "февраль ", "март ",
                        "апрель ", "май ", "июнь ", "июль ",
                        "август ", "сентябрь ", "октябрь ",
                        "ноябрь ", "декабрь "
                    };

            string[] monthGenitive = new string[]
                    {
                        "января ", "февраля ", "марта ", "апреля ",
                        "мая ", "июня ", "июля ", "августа ",
                        "сентября ", "октября ",
                        "ноября ", "декабря "
                    };

            if (value != DateTime.MinValue)
            {
                sb.Append(isGenitive ? monthGenitive[value.Month - 1] : month[value.Month - 1]);
                sb.Append(value.Year.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Converts a date to a quarter
        /// </summary>
        /// <summary>
        ///     Converts a date to a quarter
        /// </summary>
        public static int ToQuaterNum(this DateTime value)
        {
            int quter = 0;
            if (value != DateTime.MinValue)
            {
                switch (value.Month)
                {
                    case 1: case 2: case 3: quter = 1; break;
                    case 4: case 5: case 6: quter = 2; break;
                    case 7: case 8: case 9: quter = 3; break;
                    case 10: case 11: case 12: quter = 4; break;
                }
            }
            return quter;
        }

        /// <summary>
        ///     Converts a date to a week   
        /// </summary>
        public static int ToWeekNum(this DateTime value)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(value);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
                value = value.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        ///     Return DateTime by week number
        /// </summary>        
        public static DateTime GetFirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }
    }
}
