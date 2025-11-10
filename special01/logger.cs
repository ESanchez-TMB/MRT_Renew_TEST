using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace special01
{
    class logger
    {
        private static logger _logger;
        private System.IO.StreamWriter _logFile;
        private logger()
        {
            string fileName;
            string logPath = System.Configuration.ConfigurationSettings.AppSettings["logPath"];
            if (logPath == null)
            {
                fileName = "logFile" + DateTime.Now.ToString("d") + ".txt";
            }
            else
            {
                fileName = logPath + "logFile" + DateTime.Now.ToString("d") + ".txt";
            }
            fileName = fileName.Replace("/", "_");
            _logFile = new System.IO.StreamWriter(fileName);
        }
        public static logger getLogger()
        {
            if (_logger == null) _logger = new logger();
            return _logger;
        }
        public void log(string s)
        {
            _logFile.WriteLine(s);
            _logFile.Flush();
        }
        public void close()
        {
            _logFile.Close();
        }
    }
}
