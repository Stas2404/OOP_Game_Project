using System.Drawing;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;

public class Map
{
    internal const int Width = 10;
    internal const int Height = 10;
    BaseElement[,] field = new BaseElement[Width, Height];
    public Player player;
    public GameLevel GameLevel { get; private set; } = new GameLevel();
    public bool IsCustomLevel { get; set; } = false;


    public BaseElement this[int y, int x]
    {
        get => field[y, x];
        set => field[y, x] = value;
    }



    public void Init()
    {
        field = new BaseElement[Width, Height];

        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                field[i, j] = new EmptyTile();

        player = new Player(0, 0, this);
        field[0, 0] = player;

        for (int i = 0; i < 5; i++)
        {
            var pos = GetRandomFreePosition();
            field[pos.y, pos.x] = new Wall();
        }

        var range = GameLevel.GetEnemyLevelRange();
        int totalEnemies = new Random().Next(3, 6);
        int tankCount = new Random().Next(0, totalEnemies + 1);

        for (int i = 0; i < totalEnemies; i++)
        {
            var pos = GetRandomFreePosition();
            string sym = (i < tankCount) ? "T" : "E";
            var stats = new EnemyStats(new Random().Next(range.min, range.max + 1), sym == "T");
            field[pos.y, pos.x] = new Enemy(sym, Color.Red, Color.Black, stats);
        }

        Random rnd = new Random();
        if (rnd.NextDouble() < 0.2)
        {
            var pos = GetRandomFreePosition();
            field[pos.y, pos.x] = new LevelUpElement();
        }

        if (rnd.NextDouble() < 0.2)
        {
            var pos = GetRandomFreePosition();
            field[pos.y, pos.x] = new GambleElement();
        }
    }

    private (int x, int y) GetRandomFreePosition()
    {
        Random rnd = new Random();
        int x, y;

        do
        {
            x = rnd.Next(0, Width);
            y = rnd.Next(0, Height);
        }
        while (!(field[y, x] is EmptyTile) || field[y, x].Output != "." || (x == player.X && y == player.Y));

        return (x, y);
    }

    public void GenerateNextLevel(bool isCustomLevel = false)
    {
        if (!HasEnemiesLeft())
        {
            if (isCustomLevel)
            {
                Console.WriteLine("You completed the custom level!");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();

                Game game = new Game();
                game.Menu();
                return;
            }

            GameLevel.IncreaseLevel();

            field = new BaseElement[Width, Height];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    field[i, j] = new EmptyTile();

            player.SetPosition(0, 0);
            field[0, 0] = player;

            Console.WriteLine($"\n--- New game level: {GameLevel.LevelNumber} ---\n");
            Thread.Sleep(1000);

            if (GameLevel.LevelNumber == 2 || GameLevel.LevelNumber == 4)
            {
                field = MazeGenerator.GenerateMaze(Width, Height, player, GameLevel);
            }
            else if (GameLevel.LevelNumber == 5)
            {
                if (player.Level < 5)
                {
                    Console.Clear();
                    Console.WriteLine("Your level is too low to defeat the boss!");
                    Console.WriteLine("The game will be restarted...");
                    Thread.Sleep(3000);

                    GameLevel = new GameLevel();
                    player.Level = 1;
                    player.WinsSinceLastLevel = 0;

                    Init();
                    return;
                }

                int bossX = Width / 2;
                int bossY = Height / 2;

                var boss = new Enemy("B", Color.DarkRed, Color.Black, new EnemyStats(25));
                field[bossY, bossX] = boss;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    var pos = GetRandomFreePosition();
                    field[pos.y, pos.x] = new Wall();
                }

                var range = GameLevel.GetEnemyLevelRange();
                int totalEnemies = new Random().Next(3, 6);
                int tankCount = new Random().Next(0, totalEnemies + 1);

                for (int i = 0; i < totalEnemies; i++)
                {
                    var pos = GetRandomFreePosition();
                    string sym = (i < tankCount) ? "T" : "E";
                    var stats = new EnemyStats(new Random().Next(range.min, range.max + 1), sym == "T");
                    field[pos.y, pos.x] = new Enemy(sym, Color.Red, Color.Black, stats);
                }

                Random rnd = new Random();
                if (rnd.NextDouble() < 0.2)
                {
                    var pos = GetRandomFreePosition();
                    field[pos.y, pos.x] = new LevelUpElement();
                }

                if (rnd.NextDouble() < 0.2)
                {
                    var pos = GetRandomFreePosition();
                    field[pos.y, pos.x] = new GambleElement();
                }
            }
        }
    }

    public bool HasEnemiesLeft()
    {
        foreach (var elem in field)
        {
            if (elem.IsEnemy && elem.Output != ".")
                return true;
        }
        return false;
    }

    public void Print()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
                Console.Write(field[i, j].Output + " ");
            Console.WriteLine();
        }

        Console.WriteLine($"\nPlayer's level: {player.Level}");
        Console.WriteLine($"Game level: {GameLevel.LevelNumber}");
    }

    public void LoadFromFile(string path)
    {
        if (!File.Exists(path)) return;

        var lines = File.ReadAllLines(path);
        string[] playerData = lines[0].Split(',');

        int playerX = int.Parse(playerData[0]);
        int playerY = int.Parse(playerData[1]);
        int playerLevel = int.Parse(playerData[2]);
        int winsSinceLastLevel = playerData.Length > 3 ? int.Parse(playerData[3]) : 0;
        int gameLevel = int.Parse(playerData[4]);

        field = new BaseElement[Width, Height];

        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                field[i, j] = new EmptyTile();

        player = new Player(playerX, playerY, this);
        player.Level = playerLevel;
        player.WinsSinceLastLevel = winsSinceLastLevel;
        field[playerY, playerX] = player;

        GameLevel = new GameLevel();
        while (GameLevel.LevelNumber < gameLevel)
            GameLevel.IncreaseLevel();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            string type = parts[2];
            string output = parts[3];
            Color fg = Color.FromArgb(int.Parse(parts[4]));
            Color bg = Color.FromArgb(int.Parse(parts[5]));
            bool isPassable = bool.Parse(parts[6]);

            BaseElement elem;

            switch (type)
            {
                case "Wall":
                    elem = new Wall();
                    break;
                case "Enemy":
                    int level = parts.Length > 7 ? int.Parse(parts[7]) : GameLevel.GetEnemyLevelRange().min;
                    bool isTank = parts.Length > 8 && bool.Parse(parts[8]);
                    var stats = new EnemyStats(level, isTank);
                    elem = new Enemy(output, fg, bg, stats);
                    break;
                case "LevelUpElement":
                    elem = new LevelUpElement();
                    break;
                case "GambleElement":
                    elem = new GambleElement();
                    break;
                default:
                    elem = new EmptyTile();
                    break;
            }

            field[y, x] = elem;
        }
    }

    public BaseElement GetElement(int x, int y)
    {
        return field[y, x];
    }
}
