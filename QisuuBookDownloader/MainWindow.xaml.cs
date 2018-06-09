using QisuuBookDownloader.Data.Models;
using QisuuBookDownloader.Data.Services;
using QisuuBookDownloader.Data.Tasks;
using QisuuBookDownloader.Utils.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QisuuBookDownloader
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private int _messageCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task(() =>
            {

                GetBookTask task = new GetBookTask();
                Log("开始下载");
                string text = null;
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    text = this.TbUrl.Text;
                }));
                task.Logger = new Logger1(Log);
                var book = task.Get(text);
                BookMerge merge = new BookMerge();
                Log("开始合并");
                Directory.CreateDirectory("txt");
                var fileName = "txt\\" + book.Name + ".txt";
                merge.Merge(book, fileName);
                Log($"下载完成 {fileName}");
            });
            t.Start();
        }

        private void Log(string message)
        {
            _messageCount++;
            Dispatcher.Invoke(() =>
            {
                if (_messageCount == 200)
                {
                    var txt = this.TbInfo.Text;
                    this.TbInfo.Text = txt.Substring(txt.IndexOf('\n', txt.Length / 2)) + message + "\n";
                    _messageCount = 0;
                }
                else
                {
                    this.TbInfo.Text += message + "\n";
                }
            });
        }

    }
}
