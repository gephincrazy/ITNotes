using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlerLib
{
    public static class RegexTool
    {
        public static string Replace(string input, string pattern, string evaluator)
        {
            if (input == null)
            {
                return string.Empty;
            }
            else
            {
                return Regex.Replace(input, pattern, evaluator);
            }
            
        }

        public static string Match(string input, string pattern)
        {
            if (input == null)
            {
                return string.Empty;
            }
            else if (Regex.IsMatch(input, pattern))
            {
                return Regex.Match(input, pattern).Groups[0].Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
