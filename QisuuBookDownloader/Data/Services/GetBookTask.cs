using HtmlAgilityPack;
using QisuuBookDownloader.Data.Models;
using QisuuBookDownloader.Data.Services;
using QisuuBookDownloader.Utils.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace QisuuBookDownloader.Data.Tasks
{
    public class GetBookTask
    {
        private WebClient wc;
        private Encoding encoding= Encoding.UTF8;
        public ILogger Logger { get; set; }

        public GetBookTask()
        {
            wc = new WebClient();
        }

        public Book Get(string url)
        {
            var str = encoding.GetString(wc.DownloadData(url));
            var list = GetChapterUrls(str, url);
            var book = new Book { Chapters = list, Url = url };
            FetchChapters(list);
            return book;
        }

        public string GetString(byte[] data)
        {
            GZipStream stream = new GZipStream(new MemoryStream(data),CompressionMode.Decompress);
            StreamReader sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

        private void FetchChapters(List<Chapter> list)
        {
            foreach (var ch in list)
            {
                Logger?.Log($"正在下载 {ch.Title}");
                FetchChapter(ch);
                Logger?.Log($"下载完成 {ch.Title}");
            }
        }

        public class TmpData
        {
            public byte[] Data { get; set; }
            public string Name { get; set; }
            public string Content { get; set; }
        }

        private void FetchChapter(Chapter ch)
        {
            var data = wc.DownloadData(ch.Url);
            var str = encoding.GetString(data);
            try {
                ch.Content = ChapterContent.Get(str);
            } catch (Exception ex)
            {
                ch.Content = GetString(data);
            }
        }

        public List<Chapter> GetChapterUrls(string str, string url)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(str);
            var links = doc.DocumentNode.SelectNodes(@"//*[@id='info'][3]/div[1]/ul/li/a");
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
            return list;
        }
    }
}
