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
        Base = 16,
        Boss = 32,
        Crit = 64,
        Mystery = 128,
    }
}
