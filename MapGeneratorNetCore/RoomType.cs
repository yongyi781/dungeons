using System;

namespace MapGeneratorNetCore
{
    [Flags]
    public enum RoomType
    {
        Gap = -1,
        NotOpened,
        N = 1,
        S = 2,
        E = 4,
        W = 8,
        Home = 16,
        Boss = 32
    }
}
