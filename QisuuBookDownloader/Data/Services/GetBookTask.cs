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

namespace QisuuBookDownloader.Data.Tasks
{
    public class GetBookTask
    {
        private WebClient wc;
        private Encoding encoding = Encoding.UTF8;
        public ILogger Logger { get; set; }

        public GetBookTask()
        {
            wc = new WebClient();
        }

        public Book Get(string url)
        {
            var str = encoding.GetString(wc.DownloadData(url));
            var book = ParseBookHtml(str, url);
            FetchChapters(book.Chapters);
            return book;
        }

        public string GetString(byte[] data)
        {
            GZipStream stream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
            StreamReader sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

        private void FetchChapters(List<Chapter> list)
        {
            foreach (var ch in list)
            {
                Logger?.Log($"正在下载 {ch.Title}");
                while (true) {
                    try { 
                    FetchChapter(ch);
                    }catch(Exception ex)
                    {
                        Logger.Log(ex.Message);
                        continue;
                    }
                    break;
                }
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
            //下载html
            var data = wc.DownloadData(ch.Url);
            var str = encoding.GetString(data);
            try
            {
                ch.Content = ChapterContent.Get(str);
            }
            catch (Exception ex)
            {
                ch.Content = ChapterContent.Get(GetString(data));
            }
        }

        public Book ParseBookHtml(string str, string url)
        {
            var book = new Book { Url = url };
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(str);

            var nameNode = doc.DocumentNode.SelectSingleNode("//*[@id='info'][1]/div[1]/div[2]/h1");
            var authorNode = doc.DocumentNode.SelectSingleNode("//*[@id='info'][1]/div[1]/div[2]/dl[1]");
            var links = doc.DocumentNode.SelectNodes(@"//*[@id='info'][3]/div[1]/ul/li/a");
            book.Author = authorNode.InnerText.Replace("&nbsp;", "");
            book.Name = nameNode.InnerText;
            book.Chapters = new List<Chapter>();
            foreach (var link in links)
            {
                Chapter ch = new Chapter
                {
                    Title = link.InnerText,
                    Url = url + link.Attributes["href"].Value
                };
                book.Chapters.Add(ch);
            }
            return book;
        }
    }
}
