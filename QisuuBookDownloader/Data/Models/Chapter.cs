using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QisuuBookDownloader.Data.Tasks.GetBookTask;

namespace QisuuBookDownloader.Data.Models
{
    public class Chapter
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
