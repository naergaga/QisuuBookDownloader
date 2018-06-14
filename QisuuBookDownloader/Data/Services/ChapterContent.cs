using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
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
            HtmlNode node = null;
          
                node = doc.DocumentNode.SelectSingleNode("//*[@id='content1']");
           
            var str = node.InnerText.Trim().Replace("&nbsp;&nbsp;&nbsp;&nbsp;",
                "\r\n    ");
            return str;
        }
    }
}
