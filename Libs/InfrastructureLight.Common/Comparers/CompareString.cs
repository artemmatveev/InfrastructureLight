using System;
using System.Collections.Generic;

namespace InfrastructureLight.Common.Comparers
{
    public class CompareString : IComparer<string>
    {
        public int Compare(string x, string y) 
            => ComparerDigitalString(x, y);        

        private int ComparerDigitalString(string x, string y)
        {
            int d1, d2, i = 0, n, m;
            const int cNull = 48;

            while (i < x.Length && i < y.Length)
            {                
                if (char.IsNumber(x[i]) && char.IsNumber(y[i]))
                {
                    d1 = Convert.ToInt16(x[i]) - cNull;
                    n = i + 1;
                    while (n < x.Length)
                    {
                        if (char.IsNumber(x[n]))
                        {
                            d1 = d1 * 10 + Convert.ToInt16(x[n]) - cNull;
                        }
                        else
                        {
                            break;
                        }
                        n += 1;
                    }

                    d2 = Convert.ToInt16(y[i]) - cNull;
                    m = i + 1;
                    while (m < y.Length)
                    {
                        if (char.IsNumber(y[m]))
                        {
                            d2 = d2 * 10 + Convert.ToInt16(y[m]) - cNull;
                        }
                        else
                        {
                            break;
                        }
                        m += 1;
                    }

                    if (d1 < d2) return -1;
                    if (d1 > d2) return 1;
                }

                if (x[i] != y[i])
                {
                    return x[i].CompareTo(y[i]);
                }

                i += 1;
            }

            if (x.Length < y.Length) return -1;
            if (x.Length > y.Length) return 1;

            return 0;
        }
    }
}
