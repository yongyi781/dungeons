using System.Diagnostics;

namespace MapGenerator
{
    public class Logger
    {
        public static readonly Logger Global = new();

        public TextBox? TextBox { get; set; }
        public LogLevel LogLevel { get; set; }

        public void Log(LogLevel level, string text)
        {
            if (level >= LogLevel)
                LogHelper(text);
        }

        void LogHelper(string text)
        {
            TextBox?.Invoke(() => TextBox.AppendText(text + Environment.NewLine));
            Debug.WriteLine(text);
        }
    }
}
