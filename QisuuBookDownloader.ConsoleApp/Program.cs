using QisuuBookDownloader.Data.Services;
using QisuuBookDownloader.Data.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisuuBookDownloader.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GetBookTask task = new GetBookTask();
            var t = task.Get("https://www.qisuu.com/du/36/36885/");
            var book = t.GetAwaiter().GetResult();
            BookMerge merge = new BookMerge();
            merge.Merge(book, "test.txt");
        }
    }
}
