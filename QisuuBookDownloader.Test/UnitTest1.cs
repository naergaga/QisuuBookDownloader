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
            //task.Get1(File.ReadAllText(@"C:\Users\mei\Desktop\放开那个女巫.html"));
        }

        [TestMethod]
        public void GetParseChapter()
        {
            var str = ChapterContent.Get(File.ReadAllText(@"C:\Users\mei\Desktop\第一章_从今天开始做王子_放开那个女巫.html"));
            StreamWriter writer = new StreamWriter(@"C:\Users\mei\Desktop\第一章_从今天开始做王子.txt");
            writer.Write(str);
            writer.Close();
            Console.WriteLine(str);
        }
    }
}
