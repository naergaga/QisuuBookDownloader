using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisuuBookDownloader.Data.Models
{
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public List<Chapter> Chapters { get; set; }

        public override string ToString()
        {
            return " Name: " + Name + " Author: " + Author + " Url: " + Url + " Chapters: " + Chapters;
        }
    }
}
