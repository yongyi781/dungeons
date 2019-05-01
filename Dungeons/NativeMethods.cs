using System;
using System.Runtime.InteropServices;

namespace Dungeons
{
    static class NativeMethods
    {
        public const int WM_HOTKEY = 0x0312;
        public const int VK_F11 = 0x7A;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);
    }
}
