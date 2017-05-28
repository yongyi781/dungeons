using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    /// <summary>
    /// Contains all the information in a dungeoneering map, along with user annotations.
    /// </summary>
    public class Map
    {
        const int MapSize = 8;

        private RoomType[,] roomTypes = new RoomType[MapSize, MapSize];
        private int[,] distances = new int[MapSize, MapSize];
        private Point[,] parents = new Point[MapSize, MapSize];

        private Bitmap image;
        public Bitmap Image
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    image = value;
                    AnalyzeMap();
                }
            }
        }

        public string[,] Annotations = new string[8, 8];
        public int OpenedRoomCount { get; private set; }
        public Point HomeLocation { get; private set; }
        public Point BossLocation { get; private set; }
        public HashSet<Point> MarkedCriticalRooms { get; private set; } = new HashSet<Point>();
        public HashSet<Point> CriticalRooms { get; private set; } = new HashSet<Point>();

        public RoomType GetRoomType(Point p)
        {
            var pc = MapUtils.MapToClientCoords(p);
            return MapUtils.GetRoomType(Image as Bitmap, pc.X, pc.Y);
        }

        public double GetRoomsPerMinute(TimeSpan elapsed)
        {
            return (OpenedRoomCount - 1) / elapsed.TotalMinutes;
        }

        private bool IsRoom(Point p)
        {
            return MapUtils.IsValidMapCoords(p) && parents[p.X, p.Y] != MapUtils.NotFound;
        }

        private void AnalyzeMap()
        {
            HomeLocation = MapUtils.NotFound;
            BossLocation = MapUtils.NotFound;
            OpenedRoomCount = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var roomType = GetRoomType(new Point(x, y));
                    roomTypes[x, y] = roomType;
                    if (MapUtils.IsOpened(roomType))
                    {
                        ++OpenedRoomCount;
                        if (MapUtils.IsHome(roomType))
                            HomeLocation = new Point(x, y);
                        else if (MapUtils.IsBoss(roomType))
                            BossLocation = new Point(x, y);
                    }
                }
            }

            ComputeMapGraph();
            ComputeCriticalRooms();
        }

        private void ComputeMapGraph()
        {
            // Initialize
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    distances[i, j] = -1;
                    parents[i, j] = MapUtils.NotFound;
                }
            }

            var visited = new HashSet<Point>();

            void Visit(Point p, Point parent, int dist)
            {
                if (!MapUtils.IsValidMapCoords(p) || visited.Contains(p))
                    return;

                visited.Add(p);
                distances[p.X, p.Y] = dist;
                parents[p.X, p.Y] = parent;
                if ((roomTypes[p.X, p.Y] & RoomType.W) != 0)
                    Visit(new Point(p.X - 1, p.Y), p, dist + 1);
                if ((roomTypes[p.X, p.Y] & RoomType.E) != 0)
                    Visit(new Point(p.X + 1, p.Y), p, dist + 1);
                if ((roomTypes[p.X, p.Y] & RoomType.S) != 0)
                    Visit(new Point(p.X, p.Y - 1), p, dist + 1);
                if ((roomTypes[p.X, p.Y] & RoomType.N) != 0)
                    Visit(new Point(p.X, p.Y + 1), p, dist + 1);
            }

            Visit(HomeLocation, HomeLocation, 0);
        }

        private void ComputeCriticalRooms()
        {
            CriticalRooms.Clear();
            CriticalRooms.Add(HomeLocation);
            var marked = MarkedCriticalRooms.Where(p => MapUtils.IsValidMapCoords(p) && IsRoom(p));
            if (MapUtils.IsValidMapCoords(BossLocation))
                marked = marked.Union(new Point[] { BossLocation });
            foreach (var room in marked)
            {
                for (var p = room; MapUtils.IsValidMapCoords(p) && p != HomeLocation; p = parents[p.X, p.Y])
                    CriticalRooms.Add(p);
            }
        }
    }
}
