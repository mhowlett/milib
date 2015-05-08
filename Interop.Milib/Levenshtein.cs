using System;
using System.Collections.Generic;
using System.Text;

namespace Interop.Milib
{
    public class Levenshtein
    {
        /// <summary>
        ///     Computes the Levenshtein distance between two strings. 
        ///     Refer to http://en.wikipedia.org/wiki/Levenshtein_distance
        /// </summary>
        /// <param name="s">
        ///     string 1
        /// </param>
        /// <param name="t">
        ///     string 2
        /// </param>
        /// <returns>
        ///     The minimum number of changes to move from s to t.
        /// </returns>
        public static int CalculateLevenshteinDistance(string s, string t)
        {
            if (s.Length == 0)
            {
                return t.Length;
            }
            if (t.Length == 0)
            {
                return s.Length;
            }

            int[] v0 = new int[s.Length + 1];
            int[] v1 = new int[s.Length + 1];

            for (int i = 1; i <= s.Length; ++i)
            {
                v0[i] = i;
            }

            for (int j = 1; j <= t.Length; ++j)
            {
                v1[0] = j;

                for (int i = 1; i <= s.Length; ++i)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                    int m_min = v0[i] + 1;
                    int b = v1[i - 1] + 1;
                    int c = v0[i - 1] + cost;

                    if (b < m_min) m_min = b;
                    if (c < m_min) m_min = c;

                    v1[i] = m_min;
                }

                int[] vTmp = v0;
                v0 = v1;
                v1 = vTmp;
            }

            return v0[s.Length];
        }
    }
}
