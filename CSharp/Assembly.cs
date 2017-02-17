using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace QueryLog4Net
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = DoWorkAsync().Result;
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static Task<string> DoWorkAsync()
        {
            return Task<string>.Factory.StartNew(() => {
                Thread.Sleep(3000);
                return "Hello world...";
            });
        }


        static void GetLog()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = directory.Substring(0, directory.IndexOf("bin")) + "loginfo.txt";
            string text = File.ReadAllText(file);
            string outtext = "";
            // Define a regular expression for repeated words.
            Regex rx = new Regex(@"https://www.booking.com/hotel/[\w]+/[\w\-]+.zh-cn.html", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Find matches.
            MatchCollection matches = rx.Matches(text);

            // Report on each match.
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine(groups[0].Value);
                outtext = outtext + groups[0].Value + "\n";
            }

            File.WriteAllText(@"D:\VS2015Project\QueryLog4Net\QueryLog4Net\logresult.txt", outtext);
            Console.ReadKey();
        }

    }
}
