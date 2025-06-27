using System;

public class LevelEditorConsoleUI
{
    private readonly LevelEditor editor;
    private readonly IGameUI ui;

    private bool isRunning = true;

    public LevelEditorConsoleUI(LevelEditor editor, IGameUI ui)
    {
        this.editor = editor;
        this.ui = ui;
    }

    public void Run()
    {
        while (isRunning)
        {
            ui.Clear();
            DrawField();
            ShowControls();

            ConsoleKey key = ui.ReadKey();

            switch (key)
            {
                case ConsoleKey.W: editor.MoveCursor(0, -1); break;
                case ConsoleKey.S: editor.MoveCursor(0, 1); break;
                case ConsoleKey.A: editor.MoveCursor(-1, 0); break;
                case ConsoleKey.D: editor.MoveCursor(1, 0); break;

                case ConsoleKey.Enter:
                    editor.PlaceElement();
                    break;

                case ConsoleKey.F3:
                    editor.SaveToFile();
                    ui.WriteLine($"Level saved to {editor.CustomPath}");
                    ui.WaitForKey();
                    break;

                case ConsoleKey.F4:
                    var game = new Game(ui);
                    game.RunCustomLevel();
                    game.Run();
                    break;

                case ConsoleKey.Escape:
                    isRunning = false;
                    break;

                case ConsoleKey.D1:
                case ConsoleKey.D2:
                case ConsoleKey.D3:
                case ConsoleKey.D4:
                case ConsoleKey.D5:
                case ConsoleKey.D6:
                    editor.Selector.UpdateSelected(key);
                    break;
            }
        }
    }

    private void DrawField()
    {
        for (int y = 0; y < LevelEditor.Height; y++)
        {
            for (int x = 0; x < LevelEditor.Width; x++)
            {
                if (x == editor.CursorX && y == editor.CursorY)
                {
                    ui.SetColor(ConsoleColor.White, ConsoleColor.DarkGray);
                }

                ui.Write(editor.Field[y, x].Output + " ");
                ui.ResetColor();
            }
            ui.WriteLine();
        }
    }

    private void ShowControls()
    {
        ui.WriteLine($"\nChosen element: {editor.Selector.GetSelectedLabel()} | F3 - save | F4 - load | ESC - exit");
        ui.WriteLine("1 - E (enemy)");
        ui.WriteLine("2 - T (tank)");
        ui.WriteLine("3 - W (wall)");
        ui.WriteLine("4 - L (levelup)");
        ui.WriteLine("5 - G (gamble)");
        ui.WriteLine("6 - # (delete)");
    }
}
