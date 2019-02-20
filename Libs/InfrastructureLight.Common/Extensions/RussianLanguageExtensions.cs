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
