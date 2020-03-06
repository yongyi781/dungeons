using Dungeons.Common;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeons
{
    public static class MapUtils
    {
        // Size of each room on map, in pixels.
        public const int RoomSize = 32;
        public const int MapEdgeColorTolerance = 100;

        public static readonly Point NotFound = new Point(-1, -1);
        public static readonly Color MapCornerColor = Color.FromArgb(105, 92, 70);

        private static Bitmap[] Rooms;
        private static readonly List<Color[]> signatures = new List<Color[]>();

        public static void InitializeRoomsAndSignatures()
        {
            Rooms = new[] {
                Properties.Resources.RoomN,
                Properties.Resources.RoomS,
                Properties.Resources.RoomNS,
                Properties.Resources.RoomE,
                Properties.Resources.RoomEN,
                Properties.Resources.RoomES,
                Properties.Resources.RoomENS,
                Properties.Resources.RoomW,
                Properties.Resources.RoomNW,
                Properties.Resources.RoomSW,
                Properties.Resources.RoomNSW,
                Properties.Resources.RoomEW,
                Properties.Resources.RoomENW,
                Properties.Resources.RoomESW,
                Properties.Resources.RoomENSW
            };

            foreach (var room in Rooms)
            {
                signatures.Add(ComputeSignature(room));
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

        public static RoomType GetRoomType(Bitmap bmp, int offX = 0, int offY = 0)
        {
            if (bmp == null)
                return RoomType.Mystery;
            var sig = ComputeSignature(bmp, offX, offY);
            var index = signatures.FindIndex(arr => Enumerable.SequenceEqual(sig, arr));
            if (index == -1)
                return RoomType.Mystery;

            var result = (RoomType)(index + 1);
            if (bmp.GetPixel(offX + 19, offY + 18) == Color.FromArgb(150, 145, 105))
                result |= RoomType.Base;
            else if (bmp.GetPixel(offX + 8, offY + 11) == Color.FromArgb(63, 20, 13))
                result |= RoomType.Boss;

            return result;
        }

        public static bool IsValidMap(Bitmap bmp)
        {
            // Test if corners are at most tolerance away from the map corner color
            using (var u = new UnsafeBitmap(bmp))
            {
                foreach (var p in new Point[] { new Point(0, 0), new Point(bmp.Width - 1, 0), new Point(0, bmp.Height - 1), new Point(bmp.Width - 1, bmp.Height - 1) })
                {
                    if (UnsafeBitmap.ColorDistance(u.GetPixel(p.X, p.Y), MapCornerColor) > MapEdgeColorTolerance)
                        return false;
                }
            }
            return true;
        }
    }
}
