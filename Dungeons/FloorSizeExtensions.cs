using Dungeons.Common;
using System.Drawing;

namespace Dungeons
{
    public static class FloorSizeExtensions
    {
        public static Bitmap GetMapMarker(this FloorSize floorSize) => (Bitmap)Properties.Resources.ResourceManager.GetObject($"Outline{floorSize}");
    }
}
