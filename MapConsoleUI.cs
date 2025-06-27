public class MapConsoleUI
{
    private readonly Map map;
    private readonly IGameUI ui;

    public MapConsoleUI(Map map, IGameUI ui)
    {
        this.map = map;
        this.ui = ui;
    }

    public void Draw()
    {
        for (int y = 0; y < Map.Height; y++)
        {
            for (int x = 0; x < Map.Width; x++)
            {
                ui.Write(map[y, x].Output + " ");
            }
            ui.WriteLine();
        }

        ui.WriteLine($"\nPlayer's level: {map.player.Level}");
        ui.WriteLine($"Game level: {map.GameLevel.LevelNumber}");
    }
}
