using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QisuuBookDownloader.Data.Services;
using QisuuBookDownloader.Data.Tasks;

namespace QisuuBookDownloader.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetFetchChapter()
        {
            GetBookTask task = new GetBookTask();
            var book = task.ParseBookHtml(File.ReadAllText(@"C:\Users\mei\Desktop\test.html"),"");
            Console.WriteLine(book);
        }

        [TestMethod]
        public void GetParseChapter()
        {
            var str = ChapterContent.Get(File.ReadAllText(@"C:\Users\mei\Desktop\test1.html"));
            StreamWriter writer = new StreamWriter(@"C:\Users\mei\Desktop\test.txt");
            writer.Write(str);
            writer.Close();
            Console.WriteLine(str);
        }
    }
}
