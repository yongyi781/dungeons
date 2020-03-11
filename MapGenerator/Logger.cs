using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MapGenerator
{
    public class Logger
    {
        public static readonly Logger Global = new Logger();

        public TextBox TextBox { get; set; }
        public LogLevel LogLevel { get; set; }

        public void Log(LogLevel level, string text)
        {
            if (level >= LogLevel)
                LogHelper(text);
        }

        void LogHelper(string text)
        {
            if (TextBox != null)
            {
                TextBox.Invoke((Action)(() => TextBox.AppendText(text + Environment.NewLine)));
            }
            Debug.WriteLine(text);
        }
    }
}
