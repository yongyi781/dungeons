using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeons.Common
{
    public class GameMap
    {
        public GameMap(RoomType[,] roomTypes)
        {
            RoomTypes = roomTypes;

            FloorSize = new FloorSize(roomTypes.GetLength(0), roomTypes.GetLength(1));

            Map = new Map(roomTypes.GetLength(0), roomTypes.GetLength(1));

            for (int y = 0; y < roomTypes.GetLength(1); y++)
            {
                for (int x = 0; x < roomTypes.GetLength(0); x++)
                {
                    var roomType = roomTypes[x, y];
                    if (roomType.IsOpened())
                    {
                        ++OpenedRoomCount;
                        if (roomType.IsCrit())
                            Map.AddCritEndpoint(new Point(x, y));
                        if (roomType.IsBase())
                            Map.Base = new Point(x, y);
                        else if (roomType.IsBoss())
                            Map.Boss = new Point(x, y);
                        if (roomType.IsLeaf() && !roomType.IsBoss() && !roomType.IsBase())
                            ++DeadEndCount;
                    }
                }
            }

            ComputeMapData();
            Map.SimplifyCrit();
        }

        public bool IsRoom(Point p) => Map.IsRoom(p);

        private void ComputeMapData()
        {
            var visited = new HashSet<Point>();

            void Visit(Point p, Direction dir, int dist)
            {
                if (!FloorSize.IsInRange(p) || visited.Contains(p))
                    return;
                visited.Add(p);

                Map[p] = dir.Flip();

                // An reachable "gap" is probably a mystery room instead.
                if (RoomTypes[p.X, p.Y] == RoomType.Gap)
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
        public Map Map { get; }
        public FloorSize FloorSize { get; }
        public int Width => Map.Width;
        public int Height => Map.Height;
        public Point Base => Map.Base;
        public Point Boss => Map.Boss;
        public int OpenedRoomCount { get; private set; }
        public int DeadEndCount { get; private set; }
        public bool IsComplete => OpenedRoomCount > 0 && MapUtils.Range2D(FloorSize.Width, FloorSize.Height).All(p => (RoomTypes[p.X, p.Y] & RoomType.Mystery) == 0);
    }
}
