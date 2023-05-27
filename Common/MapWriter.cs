using System.Drawing;
using System.Resources;

namespace Dungeons.Common
{
    // A class to write maps to an image.
    public class MapWriter
    {
        private readonly ResourceManager resources;

        public MapWriter(ResourceManager resources)
        {
            this.resources = resources;
        }

        public Bitmap? BaseOverlay => resources.GetObject("BaseOverlay") as Bitmap;
        public Bitmap? BossOverlay => resources.GetObject("BossOverlay") as Bitmap;

        public void Draw(Graphics g, Map map, bool drawCritRooms = false, bool drawDeadEnds = true)
        {
            g.Clear(Color.Black);

            var critRooms = map.GetCritRooms();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var p = new Point(x, y);
                    if (map[p] != Direction.Gap)
                    {
                        var roomType = map.GetRoomType(p);
                        var roomBmp = RoomTypeToBitmap(drawCritRooms && critRooms.Contains(p) ? roomType | RoomType.Crit : roomType);
                        if (roomBmp != null)
                        {
                            g.DrawImage(roomBmp, x * 32, (map.Height - 1 - y) * 32, 32, 32);
                            if (drawDeadEnds && map.IsDeadEnd(p))
                            {
                                g.FillRectangle(Brushes.Red, x * 32 + 14, (map.Height - 1 - y) * 32 + 14, 4, 4);
                            }
                        }
                    }
                }
            }

            if (BaseOverlay != null)
                g.DrawImage(BaseOverlay, 32 * map.Base.X, 32 * (map.Height - 1 - map.Base.Y), 32, 32);
            if (BossOverlay != null)
                g.DrawImage(BossOverlay, 32 * map.Boss.X, 32 * (map.Height - 1 - map.Boss.Y), 32, 32);
        }

        public Bitmap WriteToImage(Map map, bool drawCritRooms = false, bool drawDeadEnds = false)
        {
            var bmp = new Bitmap(32 * map.Width, 32 * map.Height);
            using var g = Graphics.FromImage(bmp);
            Draw(g, map, drawCritRooms, drawDeadEnds);
            return bmp;
        }

        private Bitmap? RoomTypeToBitmap(RoomType type) => resources.GetObject(type.ToResourceString()) as Bitmap;
    }
}
