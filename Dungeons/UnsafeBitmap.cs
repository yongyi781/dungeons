using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dungeons
{
    /// <summary>
    /// A wrapper around bitmap for pixel-based access. The corresponding bitmap cannot be modified while an instance of this class has not been disposed.
    /// </summary>
    public sealed class UnsafeBitmap : IDisposable
    {
        public UnsafeBitmap(Bitmap bitmap, ImageLockMode flags)
        {
            Bitmap = bitmap;
            BitmapData = Bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), flags, PixelFormat.Format32bppArgb);
            Width = bitmap.Width;
            Height = bitmap.Height;
        }

        public Bitmap Bitmap { get; }
        public BitmapData BitmapData { get; }
        public int Width { get; }
        public int Height { get; }

        public unsafe int this[int x, int y] => ((int*)BitmapData.Scan0)[BitmapData.Width * y + x];

        public unsafe Color GetPixel(int x, int y) => Color.FromArgb(((int*)BitmapData.Scan0)[BitmapData.Width * y + x]);

        public void Dispose()
        {
            Bitmap.UnlockBits(BitmapData);
            GC.SuppressFinalize(this);
        }

        public static Point FindMatch(Bitmap bitmap, Bitmap template)
        {
            using (var u = new UnsafeBitmap(bitmap, ImageLockMode.ReadOnly))
                return u.FindMatch(template);
        }

        public static Point FindMatch(Bitmap bitmap, Bitmap template, Predicate<Point> condition)
        {
            using (var u = new UnsafeBitmap(bitmap, ImageLockMode.ReadOnly))
                return u.FindMatch(template, condition);
        }

        public static bool IsMatch(Bitmap bitmap, Bitmap template, int offX, int offY)
        {
            using (var u = new UnsafeBitmap(bitmap, ImageLockMode.ReadOnly))
                return u.IsMatch(template, offX, offY);
        }

        public Point FindMatch(Bitmap template)
        {
            using (var u = new UnsafeBitmap(template, ImageLockMode.ReadOnly))
                return FindMatch(u);
        }

        public Point FindMatch(Bitmap template, Predicate<Point> condition)
        {
            using (var u = new UnsafeBitmap(template, ImageLockMode.ReadOnly))
                return FindMatch(u, condition);
        }

        public Point FindMatch(UnsafeBitmap template)
        {
            for (int offY = 0; offY < Height - template.Height + 1; offY++)
                for (int offX = 0; offX < Width - template.Width + 1; offX++)
                    if (IsMatch(template, offX, offY))
                        return new Point(offX, offY);
            return new Point(-1, -1);
        }

        public Point FindMatch(UnsafeBitmap template, Predicate<Point> condition)
        {
            for (int offY = 0; offY < Height - template.Height + 1; offY++)
                for (int offX = 0; offX < Width - template.Width + 1; offX++)
                    if (IsMatch(template, offX, offY) && condition(new Point(offX, offY)))
                        return new Point(offX, offY);
            return new Point(-1, -1);
        }

        public unsafe bool IsMatch(Bitmap template, int offX, int offY)
        {
            using (var u = new UnsafeBitmap(template, ImageLockMode.ReadOnly))
                return IsMatch(u, offX, offY);
        }

        public unsafe bool IsMatch(UnsafeBitmap template, int offX, int offY)
        {
            for (int y = 0; y < template.Height; y++)
                for (int x = 0; x < template.Width; x++)
                    if (this[x + offX, y + offY] != template[x, y] && (template[x, y] & 0xFF000000) != 0)
                        return false;
            return true;
        }

        public unsafe bool IsMatchAlphaColor(UnsafeBitmap template, int offX, int offY, Color color)
        {
            for (int y = 0; y < template.Height; y++)
                for (int x = 0; x < template.Width; x++)
                    if ((template.GetPixel(x, y).A == 255 && GetPixel(x + offX, y + offY) != color) ||
                        (template.GetPixel(x, y).A != 255) && GetPixel(x + offX, y + offY) == color)
                        return false;
            return true;
        }

        public unsafe bool IsMatchAlphaColor(Bitmap template, int offX, int offY, Color color)
        {
            if (template == null)
                return false;

            using (var u = new UnsafeBitmap(template, ImageLockMode.ReadOnly))
                return IsMatchAlphaColor(u, offX, offY, color);
        }
    }
}
