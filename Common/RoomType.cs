using System;

namespace Dungeons.Common
{
    [Flags]
    public enum RoomType
    {
        Gap = -1,
        Mystery,
        N = 1,
        S = 2,
        E = 4,
        W = 8,
        Base = 16,
        Boss = 32
    }
}
