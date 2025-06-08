using System;
using System.IO;
using System.Drawing;

internal static class GameSaver
{
    public static void Save(Map map, string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            var player = map.player;
            writer.WriteLine($"{player.X},{player.Y},{player.Level},{player.WinsSinceLastLevel},{map.GameLevel.LevelNumber}");

            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    BaseElement elem = map.GetElement(x, y);
                    if (elem == player || elem is EmptyTile) continue;

                    string type = elem switch
                    {
                        Enemy => "Enemy",
                        Wall => "Wall",
                        LevelUpElement => "LevelUpElement",
                        GambleElement => "GambleElement",
                        _ => "Unknown"
                    };


                    string extraData = "";

                    if (elem is Enemy enemy)
                    {
                        extraData = $",{enemy.EnemyStats.Level},{enemy.EnemyStats.IsTank}";
                    }

                    writer.WriteLine($"{x},{y},{type},{elem.Output},{(int)elem.Forecolor.ToArgb()},{(int)elem.Backcolor.ToArgb()},{elem.IsPassable}{extraData}");
                }
            }
        }
    }

    public static void Load(Map map, string path)
    {
        if (!File.Exists(path)) return;

        map.LoadFromFile(path);
    }
}
