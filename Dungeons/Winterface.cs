using Dungeons.Common;
using System.Drawing;
using System.Text;

namespace Dungeons
{
    public class Winterface
    {
        public const int WinterfaceWidth = 512;
        public const int WinterfaceHeight = 334;

        public Winterface(UnsafeBitmap bmp, int offsetX, int offsetY)
        {
            Bitmap = bmp;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public UnsafeBitmap Bitmap { get; }
        public int OffsetY { get; }
        public int OffsetX { get; }

        public string ReadField(Field field)
        {
            StringBuilder sb = new StringBuilder();

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
                        else if (i == 10)   // Plus
                        {
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

        public void Save(string fileName)
        {
            using (var bmp = Bitmap.Bitmap.Clone(new Rectangle(OffsetX, OffsetY, WinterfaceWidth, WinterfaceHeight), Bitmap.Bitmap.PixelFormat))
            {
                bmp.Save(fileName);
            }
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
