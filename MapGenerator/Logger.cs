using System;
using System.Windows.Forms;

namespace MapGenerator
{
    public static class Logger
    {
        public static TextBox TextBox { get; set; }

        public static void Log(string text)
        {
            if (TextBox != null)
            {
                TextBox.AppendText(text + Environment.NewLine);
            }
        }
    }
}
