using System.Drawing;

namespace Dungeons
{
    public class Field
    {
        public static readonly Field Time = new() { Name = "Time", FontType = FontType.SmallFont, ForeColor = Color.FromArgb(255, 255, 255), Y = 308, StartX = 28 };
        public static readonly Field Floor = new() { Name = "Floor", FontType = FontType.BaseFont, ForeColor = Color.FromArgb(198, 155, 1), Y = 56, StartX = 78 };
        public static readonly Field FloorXP = new() { Name = "FloorXP", FontType = FontType.BaseFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 70, StartX = 47 };
        public static readonly Field PrestigeXP = new() { Name = "PrestigeXP", FontType = FontType.BaseFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 70, StartX = 147 };
        public static readonly Field BaseXP = new() { Name = "BaseXP", FontType = FontType.BaseFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 70, StartX = 247 };
        public static readonly Field SizeMod = new() { Name = "SizeMod", FontType = FontType.SmallFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 120, StartX = 295 };
        public static readonly Field BonusMod = new() { Name = "BonusMod", FontType = FontType.SmallFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 142, StartX = 298 };
        public static readonly Field DifficultyMod = new() { Name = "DifficultyMod", FontType = FontType.SmallFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 162, StartX = 156 };
        public static readonly Field LevelMod = new() { Name = "LevelMod", FontType = FontType.SmallFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 162, StartX = 298 };
        public static readonly Field FloorXPBoost = new() { Name = "FloorXPBoost", FontType = FontType.SmallFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 184, StartX = 298 };
        public static readonly Field DeathsMod = new() { Name = "DeathsMod", FontType = FontType.SmallFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 209, StartX = 298 };
        public static readonly Field TotalMod = new() { Name = "TotalMod", FontType = FontType.BaseFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 236, StartX = 126 };
        public static readonly Field FinalXP = new() { Name = "FinalXP", FontType = FontType.LargeFont, ForeColor = Color.FromArgb(226, 226, 162), Y = 271, StartX = 116 };

        public string Name { get; set; }
        public FontType FontType { get; set; }
        public Color ForeColor { get; set; }
        public int Y { get; set; }
        public int StartX { get; set; }
        public int SearchWidth { get; set; } = 50;
        public int Height => FontType.Height;
    }
}
