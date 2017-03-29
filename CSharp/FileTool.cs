using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib
{
    public static class FileTool
    {
        public static void SaveFile(string repository, string file, string content)
        {
            if (!Directory.Exists(repository))
            {
                Directory.CreateDirectory(repository);
            }
            System.IO.File.WriteAllText(repository + "\\" + file, content);
        }
    }
}
