using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisuuBookDownloader.Utils.Logger
{
    public class Logger1 : ILogger
    {
        private Action<string> _log;

        public Logger1(Action<string> Log)
        {
            _log = Log;
        }

        public void Log(string message)
        {
            _log(message);
        }
    }
}
