using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MapGenerator
{
    public static class Logger
    {
        public static TextBox TextBox { get; set; }
        public static LogLevel LogLevel { get; set; }

        public static void Log(string text, LogLevel level = LogLevel.Debug)
        {
            if (level >= LogLevel)
                LogHelper(text);
        }

        static void LogHelper(string text)
        {
            if (TextBox != null)
            {
                TextBox.Invoke((Action)(() => TextBox.AppendText(text + Environment.NewLine)));
            }
            Debug.WriteLine(text);
        }
    }
}
