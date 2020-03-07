﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeons.Common
{
    public class GameMap
    {
        public GameMap(RoomType[,] roomTypes)
        {
            RoomTypes = roomTypes;

            FloorSize = FloorSize.ByDimensions(roomTypes.GetLength(0), roomTypes.GetLength(1));

            Map = new Map(roomTypes.GetLength(0), roomTypes.GetLength(1));
            Distances = new int[Width, Height];

            for (int y = 0; y < roomTypes.GetLength(1); y++)
            {
                for (int x = 0; x < roomTypes.GetLength(0); x++)
                {
                    var roomType = roomTypes[x, y];
                    if (roomType.IsOpened())
                    {
                        ++OpenedRoomCount;
                        if (roomType.IsBase())
                            Map.Base = new Point(x, y);
                        else if (roomType.IsBoss())
                            Map.Boss = new Point(x, y);
                    }
                    if (roomType.IsLeaf() && !roomType.IsBoss() && !roomType.IsBase())
                        ++DeadEndCount;
                }
            }

            ComputeMapData();
        }

        public bool IsRoom(Point p) => Map.IsRoom(p);

        private void ComputeMapData()
        {
            // Initialize
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Distances[i, j] = int.MaxValue;
                }
            }

            var visited = new HashSet<Point>();

            void Visit(Point p, Direction dir, int dist)
            {
                if (!FloorSize.IsInRange(p) || visited.Contains(p))
                    return;
                visited.Add(p);

                Distances[p.X, p.Y] = dist;
                Map[p] = dir.Flip();
                if (RoomTypes[p.X, p.Y] == RoomType.None)
                    RoomTypes[p.X, p.Y] = RoomType.Mystery;

                foreach (var d in MapUtils.Directions)
                {
                    var roomType = d.ToRoomType();
                    if ((RoomTypes[p.X, p.Y] & roomType) != 0)
                        Visit(p.Add(d), d, dist + 1);
                }
            }

            Visit(Map.Base, Direction.None, 0);
        }

        public RoomType[,] RoomTypes { get; }
        public Map Map { get; set; }
        public FloorSize FloorSize { get; }
        public int Width => Map.Width;
        public int Height => Map.Height;
        public Point Base => Map.Base;
        public Point Boss => Map.Boss;
        public int[,] Distances { get; }
        public int OpenedRoomCount { get; set; }
        public int DeadEndCount { get; set; }
        public bool IsComplete => MapUtils.GridPoints(FloorSize.NumColumns, FloorSize.NumRows).All(p => RoomTypes[p.X, p.Y] != RoomType.Mystery);
    }
}