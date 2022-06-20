﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dungeons.Common
{
    /// <summary>
    /// A wrapper around bitmap for pixel-based access. The corresponding bitmap cannot be modified while an instance of this class has not been disposed.
    /// </summary>
    public sealed class UnsafeBitmap : IDisposable
    {
        public UnsafeBitmap(Bitmap bitmap, ImageLockMode flags = ImageLockMode.ReadOnly)
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

        public Color GetPixel(int x, int y) => Color.FromArgb(this[x, y]);

        public void Dispose()
        {
            Bitmap.UnlockBits(BitmapData);
            GC.SuppressFinalize(this);
        }

        public static Point FindMatch(Bitmap bitmap, Bitmap template, Predicate<Point> condition, int tolerance)
        {
            using var u = new UnsafeBitmap(bitmap);
            return u.FindMatch(template, condition, tolerance);
        }

        public static Point FindMapByCorners(Bitmap bitmap, Size size)
        {
            using var u = new UnsafeBitmap(bitmap);
            return u.FindMatchByCorners(size);
        }

        public static bool IsMatch(Bitmap bitmap, Bitmap template, int offX, int offY, int tolerance)
        {
            using var u = new UnsafeBitmap(bitmap);
            return u.IsMatch(template, offX, offY, tolerance);
        }

        public Point FindMatch(Bitmap template, int tolerance)
        {
            using var u = new UnsafeBitmap(template);
            return FindMatch(u, tolerance);
        }

        public Point FindMatch(Bitmap template, Predicate<Point> condition, int tolerance)
        {
            using var u = new UnsafeBitmap(template);
            return FindMatch(u, condition, tolerance);
        }

        public Point FindMatch(UnsafeBitmap template, int tolerance)
        {
            for (int offY = 0; offY < Height - template.Height + 1; offY++)
                for (int offX = 0; offX < Width - template.Width + 1; offX++)
                    if (IsMatch(template, offX, offY, tolerance))
                        return new Point(offX, offY);
            return new Point(-1, -1);
        }

        public Point FindMatch(UnsafeBitmap template, Predicate<Point> condition, int tolerance)
        {
            for (int offY = 0; offY < Height - template.Height + 1; offY++)
                for (int offX = 0; offX < Width - template.Width + 1; offX++)
                    if (IsMatch(template, offX, offY, tolerance) && condition(new Point(offX, offY)))
                        return new Point(offX, offY);
            return new Point(-1, -1);
        }

        public Point FindMatchByCorners(Size size)
        {
            for (int offY = 0; offY < Height - size.Height + 1; offY++)
                for (int offX = 0; offX < Width - size.Width + 1; offX++)
                    if (IsMatchByCorner(size, offX, offY))
                        return new Point(offX, offY);
            return new Point(-1, -1);
        }

        public bool IsMatch(Bitmap template, int offX, int offY, int tolerance)
        {
            using var u = new UnsafeBitmap(template, ImageLockMode.ReadOnly);
            return IsMatch(u, offX, offY, tolerance);
        }

        public bool IsMatch(UnsafeBitmap template, int offX, int offY, int tolerance)
        {
            for (int y = 0; y < template.Height; y++)
                for (int x = 0; x < template.Width; x++)
                    if ((template[x, y] & 0xFF000000) != 0 && ColorDistance(GetPixel(x + offX, y + offY), template.GetPixel(x, y)) > tolerance)
                        return false;
            return true;
        }

        public bool IsMatchByCorner(Size size, int offX, int offY)
        {
            foreach (var p in new Point[] { new Point(0, 0), new Point(size.Width - 1, 0), new Point(0, size.Height - 1), new Point(size.Width - 1, size.Height - 1) })
            {
                if (!GetPixel(p.X + offX, p.Y + offY).IsBetween(MapReader.MapCornerColorMin, MapReader.MapCornerColorMax))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if the template matches the image at the corresponding offset; otherwise false.
        /// Transparent pixels in the template (alpha < 255) are required to be a different color in order for it to match. 
        /// </summary>
        public bool IsMatchAlphaColor(UnsafeBitmap template, int offX, int offY, Color color, int tolerance = 20)
        {
            for (int y = 0; y < template.Height; y++)
                for (int x = 0; x < template.Width; x++)
                    if ((template.GetPixel(x, y).A == 255 && ColorDistance(GetPixel(x + offX, y + offY), color) > tolerance) ||
                        (template.GetPixel(x, y).A != 255 && GetPixel(x + offX, y + offY) == color))
                        return false;
            return true;
        }

        public bool IsMatchAlphaColor(Bitmap template, int offX, int offY, Color color)
        {
            if (template == null)
                return false;

            using UnsafeBitmap u = new UnsafeBitmap(template);
            return IsMatchAlphaColor(u, offX, offY, color);
        }

        public static int ColorDistance(Color a, Color b)
        {
            var dr = a.R - b.R;
            var dg = a.G - b.G;
            var db = a.B - b.B;
            return dr * dr + dg * dg + db * db;
        }
    }
}
