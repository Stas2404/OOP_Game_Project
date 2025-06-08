using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

public class Player : BaseElement
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public int Level { get; set; } = 1;
    public int WinsSinceLastLevel { get; set; } = 0;

    private Map map;

    public Player(int x, int y, Map map) : base("P", Color.Green, Color.Black, true)
    {
        SetPosition(x, y);
        this.map = map;
    }

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void MovePlayer(int dx, int dy)
    {
        int newX = X + dx;
        int newY = Y + dy;

        if (newX < 0 || newX >= Map.Width || newY < 0 || newY >= Map.Height)
            return;

        var field = map.Field;
        var nextElement = field[newY, newX];

        if (!nextElement.IsPassable)
            return;

        if (nextElement is Enemy enemy)
        {
            var battle = new Battle(this, enemy);
            var battleUI = new BattleConsoleUI(battle);
            battleUI.Run();

            if (!battle.PlayerWon)
                return;

            field[newY, newX] = new EmptyTile();
        }

        field[Y, X] = new EmptyTile();
        SetPosition(newX, newY);
        field[Y, X] = this;

        nextElement.Interact(this);

        SoundManager.PlayMoveSound();

        if (!map.HasEnemiesLeft())
        {
            map.GenerateNextLevel(map.IsCustomLevel);
        }
    }

    public override void Print()
    {
        Console.Write(Output);
    }

    public override void Interact(Player player)
    {
    }
}
