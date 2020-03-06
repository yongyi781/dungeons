using System.Drawing;
using System.Resources;

namespace Dungeons.Common
{
    // A class to write maps to an image.
    public class MapWriter
    {
        private static readonly Brush critBrush = new SolidBrush(Color.FromArgb(128, 128, 255, 255));
        private readonly ResourceManager resources;

        public MapWriter(ResourceManager resources)
        {
            this.resources = resources;
        }

        public Image BaseOverlay => (Image)resources.GetObject("BaseOverlay");
        public Image BossOverlay => (Image)resources.GetObject("BossOverlay");

        public void Draw(Graphics g, Map map, bool drawCritRooms = false, bool drawDeadEnds = true)
        {
            g.Clear(Color.Black);

            var critRooms = map.GetCritRooms();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var p = new Point(x, y);
                    var roomBmp = RoomTypeToBitmap(map.GetRoomType(p));
                    g.DrawImage(roomBmp, x * 32, (7 - y) * 32, 32, 32);
                    if (drawDeadEnds && map.IsDeadEnd(p))
                    {
                        g.FillRectangle(Brushes.Red, x * 32 + 14, (7 - y) * 32 + 14, 4, 4);
                    }
                    if (drawCritRooms && critRooms.Contains(p))
                    {
                        g.FillRectangle(critBrush, x * 32 + 14, (7 - y) * 32 + 14, 4, 4);
                    }
                }
            }

            if (BaseOverlay != null)
                g.DrawImage(BaseOverlay, 32 * map.Base.X, 32 * (7 - map.Base.Y));
            if (BossOverlay != null)
                g.DrawImage(BossOverlay, 32 * map.Boss.X, 32 * (7 - map.Boss.Y));
        }

        public Bitmap ToImage(Map map, bool drawCritRooms = false, bool drawDeadEnds = true)
        {
            var bmp = new Bitmap(32 * map.Width, 32 * map.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                Draw(g, map, drawCritRooms, drawDeadEnds);
            }
            return bmp;
        }

        private Bitmap RoomTypeToBitmap(RoomType type) => (Bitmap)resources.GetObject(type.ToResourceString());
    }
}
