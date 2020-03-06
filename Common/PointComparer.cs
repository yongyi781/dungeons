using System.Collections.Generic;
using System.Drawing;

namespace Dungeons.Common
{
    public class PointComparer : IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            var xc = x.X.CompareTo(y.X);
            return xc != 0 ? xc : x.Y.CompareTo(y.Y);
        }
    }
}
