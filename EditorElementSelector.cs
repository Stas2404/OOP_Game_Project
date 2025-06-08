using System;
using System.Drawing;

public class EditorElementSelector
{
    private char selectedKey = '1';

    public void UpdateSelected(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.D1: selectedKey = '1'; break;
            case ConsoleKey.D2: selectedKey = '2'; break;
            case ConsoleKey.D3: selectedKey = '3'; break;
            case ConsoleKey.D4: selectedKey = '4'; break;
            case ConsoleKey.D5: selectedKey = '5'; break;
            case ConsoleKey.D6: selectedKey = '6'; break;
        }
    }

    public BaseElement GetElement(int x, int y)
    {
        return selectedKey switch
        {
            '1' => new Enemy("E", Color.Red, Color.Black, new EnemyStats(1, false)),  
            '2' => new Enemy("T", Color.DarkRed, Color.Black, new EnemyStats(1, true)),
            '3' => new Wall(),
            '4' => new LevelUpElement(),
            '5' => new GambleElement(),
            '6' => new EmptyTile(),
            _ => new EmptyTile(),
        };
    }

    public string GetSelectedLabel()
    {
        return selectedKey switch
        {
            '1' => "E (Enemy)",
            '2' => "T (Tank)",
            '3' => "W (Wall)",
            '4' => "L (LevelUp)",
            '5' => "G (Gamble)",
            '6' => "# (Clear)",
            _ => "?"
        };
    }
}
