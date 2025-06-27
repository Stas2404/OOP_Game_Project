public class ConsoleGameUI : IGameUI
{
    public void Write(string text) => Console.Write(text);
    public void WriteLine(string text = "") => Console.WriteLine(text);
    public void Clear() => Console.Clear();
    public void WaitForKey() => Console.ReadKey(true);
    public ConsoleKey ReadKey() => Console.ReadKey(true).Key;
    public void SetColor(ConsoleColor fg, ConsoleColor bg)
    {
        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;
    }

    public void ResetColor()
    {
        Console.ResetColor();
    }

    public void DrawMap(Map map)
    {
        var mapUI = new MapConsoleUI(map, this);
        mapUI.Draw();
    }

}
