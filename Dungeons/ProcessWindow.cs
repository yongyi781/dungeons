using System;
using System.Diagnostics;
using System.Drawing;
using static Dungeons.NativeMethods;

namespace Dungeons
{
    public class ProcessWindow : IEquatable<ProcessWindow>
    {
        public ProcessWindow(Process process)
        {
            Process = process;
        }

        public Process Process { get; }
        public bool HasExited => Process.HasExited;
        public IntPtr Handle => Process.MainWindowHandle;
        public string Title => Process.MainWindowTitle;
        public Size Size
        {
            get
            {
                if (GetClientRect(Handle, out var rect))
                    return new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
                return Size.Empty;
            }
        }

        public Bitmap Capture() => Capture(new Rectangle(Point.Empty, Size));

        public Bitmap Capture(Rectangle region)
        {
            if (Handle == IntPtr.Zero)
                return null;
            var size = region.Size;

            var desktopDC = GetDC(Handle);
            var memoryDC = CreateCompatibleDC(desktopDC);
            var bmp = CreateCompatibleBitmap(desktopDC, size.Width, size.Height);
            var oldBitmap = SelectObject(memoryDC, bmp);
            var success = BitBlt(memoryDC, 0, 0, size.Width, size.Height, desktopDC, region.X, region.Y, SRCCOPY | CAPTUREBLT);

            if (success)
            {
                var result = Image.FromHbitmap(bmp);
                SelectObject(memoryDC, oldBitmap);
                DeleteObject(bmp);
                DeleteDC(memoryDC);
                ReleaseDC(Handle, desktopDC);
                return result;
            }

            return null;
        }

        public bool Equals(ProcessWindow other)
        {
            return Process.Id == other.Process.Id;
        }

        public override string ToString()
        {
            var size = Size;
            return $"[{size.Width}x{size.Height}] {Title}";
        }

        public override bool Equals(object obj)
        {
            return obj is ProcessWindow x && Equals(x);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
