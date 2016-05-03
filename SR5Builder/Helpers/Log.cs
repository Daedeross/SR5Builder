using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SR5Builder.Helpers
{
    public static class Log
    {
        private static string logfile;

        private static StreamWriter writer; 

        public static void Initialize()
        {
            //Create Log folder if needed.
            DirectoryInfo dir = new DirectoryInfo(Properties.Settings.Default.ErrorLogPath);
            if (!dir.Exists)
            {
                dir.Create();
            }

            logfile = Properties.Settings.Default.ErrorLogPath + DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HH'_'mm'_'ss") + ".log";
            writer = new StreamWriter(logfile);
        }

        #region Public Methods

        public static void Close()
        {
            writer.Close();
        }

        public static void LogMessage(string message)
        {
            if (writer == null)
                Initialize();

            message = Timestamp() + " — " + message;
            
            writer.WriteLine(message);

            writer.Flush();

            System.Diagnostics.Debug.WriteLine(message);
        }

        public static void LogMessage(string message, params object[] list)
        {
            LogMessage(string.Format(message, list));
        }

        public static string Timestamp()
        {
            return DateTime.Now.ToString("HH':'mm':'ss");
        }

        #endregion Public Methods
    }
}
