public class LevelEditor
{
    public const int Width = 10;
    public const int Height = 10;

    private readonly IGameUI ui;
    public BaseElement[,] Field { get; } = new BaseElement[Height, Width];
    public int CursorX { get; private set; } = 0;
    public int CursorY { get; private set; } = 0;

    public EditorElementSelector Selector { get; } = new EditorElementSelector();

    public string CustomPath { get; }

    public LevelEditor(IGameUI ui)
    {
        CustomPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customlvl.txt");
        this.ui = ui;

        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                Field[y, x] = new EmptyTile();

        Field[0, 0] = new Player(0, 0, null, ui, null);
    }

    public void MoveCursor(int dx, int dy)
    {
        CursorX = Math.Clamp(CursorX + dx, 0, Width - 1);
        CursorY = Math.Clamp(CursorY + dy, 0, Height - 1);
    }

    public void PlaceElement()
    {
        if (CursorX == 0 && CursorY == 0) return;
        var element = Selector.GetElement(CursorX, CursorY);
        Field[CursorY, CursorX] = element;
    }

    public void SaveToFile()
    {
        using StreamWriter writer = new StreamWriter(CustomPath);
        writer.WriteLine("0,0,1,0,1"); 

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var elem = Field[y, x];
                if (elem is Player || elem is EmptyTile) continue;

                string type = elem switch
                {
                    Enemy => "Enemy",
                    Wall => "Wall",
                    LevelUpElement => "LevelUpElement",
                    GambleElement => "GambleElement",
                    _ => "Unknown"
                };

                string extra = "";

                if (elem is Enemy enemy)
                    extra = $",{enemy.EnemyStats.Level},{enemy.EnemyStats.IsTank}";

                writer.WriteLine($"{x},{y},{type},{elem.Output},{(int)elem.Forecolor.ToArgb()},{(int)elem.Backcolor.ToArgb()},{elem.IsPassable}{extra}");
            }
        }
    }
}
