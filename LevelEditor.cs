using System;
using System.Drawing;
using System.IO;

public class LevelEditor
{
    private const int Width = 10;
    private const int Height = 10;
    private BaseElement[,] field = new BaseElement[Height, Width];
    private int cursorX = 0;
    private int cursorY = 0;

    private EditorElementSelector selector = new EditorElementSelector();

    private string customPath;

    public LevelEditor()
    {
        customPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customlvl.txt");

        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                field[y, x] = new EmptyTile();

        field[0, 0] = new Player(0, 0, null);
    }

    public void Start()
    {
        bool editorRunning = true;
        while (editorRunning)
        {
            Console.Clear();
            DrawField();

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.D2:
                case ConsoleKey.D3:
                case ConsoleKey.D4:
                case ConsoleKey.D5:
                case ConsoleKey.D6:
                    selector.UpdateSelected(key.Key);
                    break;

                case ConsoleKey.W: if (cursorY > 0) cursorY--; break;
                case ConsoleKey.S: if (cursorY < Height - 1) cursorY++; break;
                case ConsoleKey.A: if (cursorX > 0) cursorX--; break;
                case ConsoleKey.D: if (cursorX < Width - 1) cursorX++; break;

                case ConsoleKey.Enter:
                    if (cursorX != 0 || cursorY != 0)
                    {
                        var element = selector.GetElement(cursorX, cursorY);
                        field[cursorY, cursorX] = element;
                    }
                    break;

                case ConsoleKey.F3:
                    SaveToFile(customPath);
                    Console.WriteLine($"Level was saved to {customPath}");
                    Console.ReadKey();
                    break;

                case ConsoleKey.F4:
                    Console.Clear();
                    var game = new Game();
                    game.RunCustomLevel();
                    break;

                case ConsoleKey.Escape:
                    editorRunning = false;
                    break;
            }
        }
    }

    private void DrawField()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (x == cursorX && y == cursorY)
                    Console.BackgroundColor = ConsoleColor.DarkGray;

                Console.Write(field[y, x].Output + " ");
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        Console.WriteLine($"\nChosen element: {selector.GetSelectedLabel()} | F3 - save | F4 - load | ESC - exit");
        Console.WriteLine("1 - E (enemy)");
        Console.WriteLine("2 - T (tank)");
        Console.WriteLine("3 - W (wall)");
        Console.WriteLine("4 - L (levelup)");
        Console.WriteLine("5 - G (gamble)");
        Console.WriteLine("6 - # (delete)");
    }

    private void SaveToFile(string path)
    {
        using StreamWriter writer = new StreamWriter(path);
        writer.WriteLine("0,0,1,0,1");

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var elem = field[y, x];
                if (elem is Player || elem is EmptyTile) continue;

                string type = elem switch
                {
                    Enemy => "Enemy",
                    Wall => "Wall",
                    LevelUpElement => "LevelUpElement",
                    GambleElement => "GambleElement",
                    _ => "Unknown"
                };

                switch (elem) 
                {
                    case Enemy:
                        type = "Nemey";
                        break;
                }

                string extra = "";

                if (elem is Enemy enemy)
                {
                    extra = $",{enemy.EnemyStats.Level},{enemy.EnemyStats.IsTank}";
                }

                writer.WriteLine($"{x},{y},{type},{elem.Output},{(int)elem.Forecolor.ToArgb()},{(int)elem.Backcolor.ToArgb()},{elem.IsPassable}{extra}");
            }
        }
    }
}
