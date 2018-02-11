using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisuuBookDownloader.Data.Models
{
    public class Book
    {
        public string Url { get; set; }
        public List<Chapter> Chapters { get; set; }
    }
}
