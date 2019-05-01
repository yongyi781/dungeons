using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeons
{
    public class FontType
    {
        public static FontType BaseFont;
        public static FontType SmallFont;
        public static FontType LargeFont;

        public List<Bitmap> NumberImageList { get; set; }
        public int Height => NumberImageList[0].Height;

        public static void InitializeFonts()
        {
            BaseFont = new FontType { NumberImageList = GetNumberList("Base").ToList() };
            SmallFont = new FontType { NumberImageList = GetNumberList("Small").Concat(new Bitmap[] { Properties.Resources.SmallPlus, Properties.Resources.SmallMinus, Properties.Resources.SmallColon }).ToList() };
            LargeFont = new FontType { NumberImageList = GetNumberList("Large").ToList() };
        }

        static IEnumerable<Bitmap> GetNumberList(string prefix) =>
            from i in Enumerable.Range(0, 10)
            select (Bitmap)Properties.Resources.ResourceManager.GetObject(prefix + i);
    }
}
