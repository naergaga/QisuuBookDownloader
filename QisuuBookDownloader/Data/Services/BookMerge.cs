using QisuuBookDownloader.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisuuBookDownloader.Data.Services
{
    public class BookMerge
    {
        public void Merge(Book book,string path)
        {
            StreamWriter writer = new StreamWriter(path);
            foreach (var item in book.Chapters)
            {
                writer.Write(item.Title);
                writer.Write(item.Content);
            }
            writer.Close();
            writer.Dispose();
        }
    }
}
