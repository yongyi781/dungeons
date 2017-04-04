using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    public static class MapUtils
    {
        const int MapOffsetX = 2;
        const int MapOffsetY = 2;
        const int MapWidth = 318;
        const int MapHeight = 310;
        const int GridOffsetX = 29;
        const int GridOffsetY = 27;
        const int SquareSize = 32;

        private static readonly Bitmap[] Rooms =
        {
            Properties.Resources.RoomN,
            Properties.Resources.RoomS,
            Properties.Resources.RoomSN,
            Properties.Resources.RoomE,
            Properties.Resources.RoomEN,
            Properties.Resources.RoomES,
            Properties.Resources.RoomESN,
            Properties.Resources.RoomW,
            Properties.Resources.RoomWN,
            Properties.Resources.RoomWS,
            Properties.Resources.RoomWSN,
            Properties.Resources.RoomWE,
            Properties.Resources.RoomWEN,
            Properties.Resources.RoomWES,
            Properties.Resources.RoomWESN
        };

        private static List<Color[]> signatures = new List<Color[]>();
        
        public static void InitializeSignatures()
        {
            foreach (var room in Rooms)
            {
                signatures.Add(ComputeSignature(room));
            }
        }

        public static Point ClientToMapCoords(Point p)
        {
            return new Point(1 + (p.X - GridOffsetX) / SquareSize, 8 - (p.Y - GridOffsetY) / SquareSize);
        }

        // Returns the upper-left corner of the square at p.
        public static Point MapToClientCoords(Point p)
        {
            return new Point((p.X - 1) * SquareSize + GridOffsetX, (8 - p.Y) * SquareSize + GridOffsetY);
        }

        public static bool IsValidMapCoords(Point p)
        {
            return p.X >= 1 && p.X <= 8 && p.Y >= 1 && p.Y <= 8;
        }

        public static bool IsOpened(RoomType t)
        {
            return t > RoomType.NotOpened;
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
                return RoomType.NotOpened;
            var sig = ComputeSignature(bmp, offX, offY);
            var index = signatures.FindIndex(arr => Enumerable.SequenceEqual(sig, arr));

            return index == -1 ? RoomType.NotOpened : (RoomType)(index + 1);
        }
    }
}
