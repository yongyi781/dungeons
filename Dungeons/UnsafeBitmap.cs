using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dungeons
{
    /// <summary>
    /// A wrapper around bitmap for pixel-based access. The corresponding bitmap cannot be modified while an instance of this class has not been disposed.
    /// </summary>
    public class UnsafeBitmap : IDisposable
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

        public unsafe Color this[int x, int y] => Color.FromArgb(((int*)BitmapData.Scan0)[BitmapData.Width * y + x]);

        public void Dispose()
        {
            Bitmap.UnlockBits(BitmapData);
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
                    if (this[x + offX, y + offY] != template[x, y])
                        return false;
            return true;
        }
    }
}
