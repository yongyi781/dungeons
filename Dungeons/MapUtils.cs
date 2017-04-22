﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    public static class MapUtils
    {
        public const int MapOffsetX = 2;
        public const int MapOffsetY = 2;
        public const int MapWidth = 318;
        public const int MapHeight = 310;
        public const int GridOffsetX = 29;
        public const int GridOffsetY = 27;
        public const int RoomWidth = 32;

        public static readonly Point NotFound = new Point(-1, -1);

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
            return new Point((p.X - GridOffsetX) / RoomWidth, 7 - (p.Y - GridOffsetY) / RoomWidth);
        }

        // Returns the upper-left corner of the square at p.
        public static Point MapToClientCoords(Point p)
        {
            return new Point(p.X * RoomWidth + GridOffsetX, (7 - p.Y) * RoomWidth + GridOffsetY);
        }

        public static bool IsValidMapCoords(Point p)
        {
            return p.X >= 0 && p.X < 8 && p.Y >= 0 && p.Y < 8;
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
            if (index == -1)
                return RoomType.NotOpened;

            var result = (RoomType)(index + 1);
            if (bmp.GetPixel(offX + 19, offY + 18) == Color.FromArgb(150, 145, 105))
                result |= RoomType.Home;
            else if (bmp.GetPixel(offX + 8, offY + 11) == Color.FromArgb(63, 20, 13))
                result |= RoomType.Boss;

            return result;
        }

        public static bool IsLeaf(RoomType roomType)
        {
            var type = roomType & (RoomType.W | RoomType.E | RoomType.S | RoomType.N);
            return type == RoomType.W || roomType == RoomType.E || roomType == RoomType.S || roomType == RoomType.N;
        }

        public static bool IsHome(RoomType roomType)
        {
            return (roomType & RoomType.Home) != 0;
        }

        public static bool IsBoss(RoomType roomType)
        {
            return (roomType & RoomType.Boss) != 0;
        }
    }
}
