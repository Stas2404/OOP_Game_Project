public interface IGameUI
{
    void Write(string text);
    void WriteLine(string text = "");
    void Clear();
    void WaitForKey();
    ConsoleKey ReadKey();
    void SetColor(ConsoleColor fg, ConsoleColor bg);
    void ResetColor();

    void DrawMap(Map map);
}
