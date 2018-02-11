using HtmlAgilityPack;
using QisuuBookDownloader.Data.Models;
using QisuuBookDownloader.Data.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace QisuuBookDownloader.Data.Tasks
{
    public class GetBookTask
    {
        private int _count = 0;
        private ActionBlock<Chapter> a1;

        public GetBookTask()
        {

        }

        public Task<Book> Get(string url)
        {
            WebClient wc = new WebClient();
            var str = Encoding.UTF8.GetString(wc.DownloadData(url));
            var list = GetChapterUrls(str, url);
            var book = new Book { Chapters = list, Url = url };
            var task = FetchChapters(list);
            return task.ContinueWith(t =>
            {
                return book;
            });
        }

        #region Test Method
        public Task Get1(string str)
        {
            return FetchChapters(GetChapterUrls(str, string.Empty));
        }

        private void FetchChapter1(Chapter ch)
        {
            Console.WriteLine($"{ch.Title} 正在下载 {ch.Url}");
            Task.Delay(5000).GetAwaiter().GetResult();
            Console.WriteLine($"{ch.Title} 下载完成。");
        }
        #endregion

        private Task FetchChapters(List<Chapter> list)
        {
            ExecutionDataflowBlockOptions dataflowBlockOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 10 };
            a1 = new ActionBlock<Chapter>(new Action<Chapter>(FetchChapter), dataflowBlockOptions);
            foreach (var ch in list)
            {
                a1.Post(ch);
            }
            Task t = new Task(() =>
            {
                while (true)
                {
                    Console.WriteLine($"{a1.Completion.IsCompleted} {a1.InputCount}");
                    Task.Delay(5000).GetAwaiter().GetResult();
                }
            });
            t.Start();
            return a1.Completion;
        }


        private void FetchChapter(Chapter ch)
        {
            Task t = new Task(() =>
            {
                while (true)
                {
                    Console.WriteLine($"em {ch.Title}");
                    Task.Delay(5000).GetAwaiter().GetResult();
                }
            });
            t.Start();
            Console.WriteLine($"{ch.Title} 正在下载");
            WebClient wc = new WebClient();
            while (true)
            {
                try
                {
                    var str = Encoding.UTF8.GetString(wc.DownloadData(ch.Url));
                    ch.Content = ChapterContent.Get(str);
                }
                catch
                {
                    Console.WriteLine($"{ch.Title} 重试");
                    continue;
                }
                Console.WriteLine($"{ch.Title} 下载完成。");
                Directory.CreateDirectory("txt");
                File.WriteAllText("txt/" + ch.Title + ".txt", ch.Content);
                break;
            }
        }

        public List<Chapter> GetChapterUrls(string str, string url)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(str);
            var links = doc.DocumentNode.SelectNodes(@"//*[@id='info']/div[1]/ul/li/a");
            List<Chapter> list = new List<Chapter>();
            foreach (var link in links)
            {
                Chapter ch = new Chapter
                {
                    Title = link.InnerText,
                    Url = url + link.Attributes["href"].Value
                };
                list.Add(ch);
            }
            list.Reverse();
            return list;
        }
    }
}
