using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the Russian Language
    /// </summary>
    public static class RussianLanguageExtensions
    {
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
        public static string ToQuaterString(this DateTime value)
        {
            var sb = new StringBuilder();

            string[] quater = new string[] { "1 квартал ", "2 квартал ", "3 квартал ", "4 квартал " };
            if (value != DateTime.MinValue)
            {
                switch (value.Month)
                {
                    case 1:
                    case 2:
                    case 3:
                        sb.Append(quater[1]);
                        break;
                    case 4:
                    case 5:
                    case 6:
                        sb.Append(quater[2]);
                        break;
                    case 7:
                    case 8:
                    case 9:
                        sb.Append(quater[3]);
                        break;
                    case 10:
                    case 11:
                    case 12:
                        sb.Append(quater[4]);
                        break;
                }
                sb.Append(value.Year.ToString().Substring(2, 2));
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Converts a date to a week   
        /// </summary>
        public static string ToWeekString(this DateTime value)
        {
            var sb = new StringBuilder();
            if (value != DateTime.MinValue)
            {
                int a = (14 - value.Month) / 12,
                    y = value.Year + 4800 - a,
                    m = value.Month + 12 * a - 3,
                    J = value.Day + (153 * m + 2) / 5 + 365 * y + y / 4 - 32083,
                    d4 = (J + 31741 - (J % 7)) % 146097 % 36524 % 1461,
                    L = d4 / 1460,
                    d1 = ((d4 - L) % 365) + L,
                    WN = d1 / 7 + 1;

                sb.AppendFormat("{0} нед. {1}", WN, value.Year.ToString().Substring(2, 2));
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Normalize string
        /// </summary>        
        public static string ToNormalizeString(this string value)
        {
            string Ru = "А|В|С|Е|Н|К|М|О|Р|Т|Х";
            string En = "ABCEHKMOPTX";
            var lookup = new Dictionary<char, string>();

            Regex pattern = new Regex(@"[\W]");
            var result = pattern.Replace(value, "");

            if (!lookup.Any())
            {
                var replace = Ru.Split('|');
                for (int i = 0; i < En.Length; ++i)
                {
                    lookup.Add(En[i], replace[i]);
                }
            }

            // Get result:
            var bf = new StringBuilder(result.Length);
            foreach (char ch in result)
            {
                if (lookup.ContainsKey(ch))
                {
                    bf.Append(lookup[ch]);
                }
                else
                {
                    bf.Append(ch);
                }
            }

            return bf.ToString().Trim();
        }
    }
}
