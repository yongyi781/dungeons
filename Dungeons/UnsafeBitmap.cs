using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dungeons
{
    public class UnsafeBitmap : IDisposable
    {
        public UnsafeBitmap(Bitmap bitmap, ImageLockMode flags, PixelFormat pixelFormat)
        {
            Bitmap = bitmap;
            BitmapData = Bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), flags, pixelFormat);
        }

        public Bitmap Bitmap { get; }
        public BitmapData BitmapData { get; }

        public void Dispose()
        {
            Bitmap.UnlockBits(BitmapData);
        }
    }
}
