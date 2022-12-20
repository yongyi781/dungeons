using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;

namespace Dungeons.Common
{
    // A class to read maps from an image.
    public class MapReader
    {
        public static readonly Color MapCornerColorMin = Color.FromArgb(100, 87, 65);
        public static readonly Color MapCornerColorMax = Color.FromArgb(117, 104, 83);

        private readonly Dictionary<RoomType, Color[]> signatures = new();

        public MapReader(ResourceManager resources)
        {
            // Normal and crit rooms
            foreach (var roomType in MapUtils.EnumerateRoomTypes())
            {
                signatures.Add(roomType, ComputeSignature((Bitmap)resources.GetObject(roomType.ToResourceString())!));
                var critRoomType = roomType | RoomType.Crit;
                if (resources.GetObject(critRoomType.ToResourceString()) is Bitmap critBmp)
                    signatures.Add(critRoomType, ComputeSignature(critBmp));
            }
            foreach (var dir in MapUtils.Directions)
            {
                var roomType = dir.ToRoomType() | RoomType.Mystery;
                signatures.Add(roomType, ComputeSignature((Bitmap)resources.GetObject(roomType.ToResourceString())!));
            }
        }

        public static Color[] ComputeSignature(Bitmap bmp, int offX = 0, int offY = 0)
        {
            return new Color[]
            {
                bmp.GetPixel(offX + 6, offY + 7),
                bmp.GetPixel(offX + 7, offY + 7),
                bmp.GetPixel(offX + 6, offY + 8),
                bmp.GetPixel(offX + 7, offY + 8)
            };
        }

        public GameMap ReadMap(Bitmap image, FloorSize floorSize)
        {
            var roomTypes = new RoomType[floorSize.Width, floorSize.Height];
            for (int y = 0; y < floorSize.Height; y++)
                for (int x = 0; x < floorSize.Width; x++)
                    roomTypes[x, y] = ReadRoom(image, new Point(x, y), floorSize);

            return new GameMap(roomTypes);
        }

        public RoomType ReadRoom(Bitmap image, Point p, FloorSize floorSize)
        {
            if (image == null)
                return RoomType.Gap;

            var pc = floorSize.MapToClientCoords(p, image.Size);
            return ReadRoom(image, pc.X, pc.Y);
        }

        public RoomType ReadRoom(Bitmap image, int offX = 0, int offY = 0)
        {
            if (image == null)
                return RoomType.Gap;

            var sig = ComputeSignature(image, offX, offY);
            var result = signatures.FirstOrDefault(pair => Enumerable.SequenceEqual(sig, pair.Value)).Key;
#if false
            if (result == 0)
            {
                image.Clone(new Rectangle(offX, offY, 32, 32), System.Drawing.Imaging.PixelFormat.Format32bppArgb).Save("out\\" + Path.ChangeExtension(Path.GetRandomFileName(), "png"));
            }
#endif
            if (image.GetPixel(offX + 19, offY + 18) == Color.FromArgb(150, 145, 105))
                result |= RoomType.Base;
            else if (image.GetPixel(offX + 8, offY + 11) == Color.FromArgb(63, 20, 13))
                result |= RoomType.Boss;

            return result;
        }

        // Check if an in-game image most likely contains a map.
        public static bool IsValidInGameMap(Bitmap bmp)
        {
            // Test if corners are at most tolerance away from the map corner color
            using var u = new UnsafeBitmap(bmp);
            foreach (var p in new Point[] { new Point(0, 0), new Point(bmp.Width - 1, 0), new Point(0, bmp.Height - 1), new Point(bmp.Width - 1, bmp.Height - 1) })
            {
                if (!u.GetPixel(p.X, p.Y).IsBetween(MapCornerColorMin, MapCornerColorMax))
                    return false;
            }
            return true;
        }
    }
}
