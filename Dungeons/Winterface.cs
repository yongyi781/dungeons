using Dungeons.Common;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.Layout;

namespace Dungeons
{
    public class Winterface(UnsafeBitmap bmp, int offsetX, int offsetY)
    {
        public const int WinterfaceWidth = 512;
        public const int WinterfaceHeight = 334;

        public UnsafeBitmap Bitmap { get; } = bmp;
        public int OffsetY { get; } = offsetY;
        public int OffsetX { get; } = offsetX;

        public string ReadField(Field field)
        {
            StringBuilder sb = new();

            int x = FindStartX(field);

            while (true)
            {
                bool found = false;
                for (int i = 0; i < field.FontType.NumberImageList.Count; i++)
                {
                    var numberImage = field.FontType.NumberImageList[i];
                    if (Bitmap.IsMatchAlphaColor(numberImage, x + OffsetX, field.Y + OffsetY, field.ForeColor))
                    {
                        found = true;
                        if (i <= 9)
                        {
                            sb.Append(i);
                        }
                        else if (i == 10)   // Plus if small, comma if large. Ignore commas.
                        {
                            if (field.FontType == FontType.SmallFont)
                                sb.Append("+");
                        }
                        else if (i == 11)   // Minus
                        {
                            sb.Append("-");
                        }
                        else if (i == 12)   // Colon
                        {
                            sb.Append(":");
                        }
                        x += numberImage.Width;
                        break;
                    }
                }
                if (!found)
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the fraction of bonus rooms opened according to the bar.
        /// Last non-black pixel at (115, 146) means 0% bonus. At (294, 146) means 100% bonus.
        /// </summary>
        /// <returns>Between 0 and 1.</returns>
        public double GetBonus()
        {
            const int y = 146;
            const int begin = 115, end = 295;
            int low_x = begin, high_x = end;

            bool pass(int x) => GetPixel(x, y).R >= 142;

            if (!pass(low_x + 1))
                return 0;
            if (pass(high_x - 1))
                return 1;
            while (high_x - low_x > 1)
            {
                int mid = low_x + (high_x - low_x) / 2;
                if (pass(mid))
                    low_x = mid;
                else
                    high_x = mid;
            }
            return ((double)low_x - begin + 1) / (end - begin);
        }

        public void Save(string fileName)
        {
            using var bmp = Bitmap.Bitmap.Clone(new Rectangle(OffsetX, OffsetY, WinterfaceWidth, WinterfaceHeight), Bitmap.Bitmap.PixelFormat);
            bmp.Save(fileName);
        }

        private Color GetPixel(int x, int y) => Bitmap.GetPixel(x + OffsetX, y + OffsetY);

        private int FindStartX(Field field)
        {
            for (int x = field.StartX; x < field.StartX + field.SearchWidth; x++)
            {
                for (var y = field.Y; y < field.Y + field.Height; y++)
                {
                    if (GetPixel(x, y) == field.ForeColor)
                        return x;
                }
            }
            return -1;
        }
    }
}
