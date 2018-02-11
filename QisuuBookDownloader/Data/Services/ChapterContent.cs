using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisuuBookDownloader.Data.Services
{
    public class ChapterContent
    {
        public static string Get(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode.SelectSingleNode("//*[@id='content1']");
            var str= node.InnerText.Replace("&nbsp;&nbsp;&nbsp;&nbsp;",
                "    ").Trim().Replace("\r\r","\r\n");
            return str;
        }
    }
}
