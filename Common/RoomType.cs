using System;

namespace Dungeons.Common
{
    [Flags]
    public enum RoomType
    {
        Gap,
        E = 1,
        N = 2,
        S = 4,
        W = 8,
        Mystery = 16,
        Crit = 32,
        Base = 64,
        Boss = 128,
    }
}
