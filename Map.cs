using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class Map
{
    public const int Width = 10;
    public const int Height = 10;

    private readonly IGameUI ui;
    private BaseElement[,] field = new BaseElement[Height, Width];
    private readonly object? form;
    private readonly Game game;

    public Player player;
    public GameLevel GameLevel { get; private set; } = new GameLevel();
    public bool IsCustomLevel { get; set; } = false;

    public Map(IGameUI ui, Game game, object? form = null)
    {
        this.ui = ui;
        this.game = game;
        this.form = form;
    }

    public BaseElement this[int y, int x]
    {
        get => field[y, x];
        set => field[y, x] = value;
    }

    public void Init()
    {
        field = new BaseElement[Height, Width];

        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                this[y, x] = new EmptyTile();

        player = new Player(0, 0, this, ui, game, form);
        this[0, 0] = player;

        GenerateObstaclesAndEnemies();
    }

    private void GenerateObstaclesAndEnemies()
    {
        for (int i = 0; i < 5; i++)
        {
            var pos = GetRandomFreePosition();
            this[pos.y, pos.x] = new Wall();
        }

        var range = GameLevel.GetEnemyLevelRange();
        int totalEnemies = new Random().Next(3, 6);
        int tankCount = new Random().Next(0, totalEnemies + 1);

        for (int i = 0; i < totalEnemies; i++)
        {
            var pos = GetRandomFreePosition();
            string sym = (i < tankCount) ? "T" : "E";
            var stats = new EnemyStats(new Random().Next(range.min, range.max + 1), sym == "T");
            this[pos.y, pos.x] = new Enemy(sym, Color.Red, Color.Black, stats);
        }

        Random rnd = new Random();

        if (rnd.NextDouble() < 0.2)
            this[GetRandomFreePosition().y, GetRandomFreePosition().x] = new LevelUpElement();

        if (rnd.NextDouble() < 0.2)
            this[GetRandomFreePosition().y, GetRandomFreePosition().x] = new GambleElement();
    }

    public void GenerateNextLevel(bool isCustomLevel = false)
    {
        if (!HasEnemiesLeft())
        {
            if (isCustomLevel)
            {
                ShowMessage("🎉 You completed the custom level!");
                TryInvokeFormMethod("ReturnToMainMenu");
                return;
            }

            if (GameLevel.LevelNumber == 5)
                return;

            GameLevel.IncreaseLevel();
            field = new BaseElement[Height, Width];

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    field[y, x] = new EmptyTile();

            player.SetPosition(0, 0);
            field[0, 0] = player;

            if (GameLevel.LevelNumber == 2 || GameLevel.LevelNumber == 4)
            {
                field = MazeGenerator.GenerateMaze(Width, Height, player, GameLevel);
            }
            else if (GameLevel.LevelNumber == 5)
            {
                if (player.Level < 5)
                {
                    ShowMessage("Your level is too low to fight the boss! Restarting the game.");
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
                GenerateObstaclesAndEnemies();
            }

            TryInvalidateForm();
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
        } while (!(this[y, x] is EmptyTile) || this[y, x].Output != "." || (x == player.X && y == player.Y));

        return (x, y);
    }

    public bool HasEnemiesLeft()
    {
        foreach (var elem in field)
            if (elem.IsEnemy && elem.Output != ".")
                return true;

        return false;
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

        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                this[y, x] = new EmptyTile();

        player = new Player(playerX, playerY, this, ui, game, form);
        player.Level = playerLevel;
        player.WinsSinceLastLevel = winsSinceLastLevel;
        this[playerY, playerX] = player;

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

            BaseElement elem = type switch
            {
                "Wall" => new Wall(),
                "Enemy" => new Enemy(output, fg, bg,
                    new EnemyStats(int.Parse(parts[7]), bool.Parse(parts[8]))),
                "LevelUpElement" => new LevelUpElement(),
                "GambleElement" => new GambleElement(),
                _ => new EmptyTile()
            };

            this[y, x] = elem;
        }
    }

    public BaseElement GetElement(int x, int y)
    {
        return this[y, x];
    }

    private void TryInvalidateForm()
    {
        if (form == null) return;

        var method = form.GetType().GetMethod("Invalidate", Type.EmptyTypes);
        method?.Invoke(form, null);
    }

    private void TryInvokeFormMethod(string methodName)
    {
        if (form == null) return;

        var method = form.GetType().GetMethod(methodName, Type.EmptyTypes);
        method?.Invoke(form, null);
    }

    private void ShowMessage(string message)
    {
        if (form == null)
            ui.WriteLine(message);
        else
        {
            var method = form.GetType().GetMethod("AppendMessage", new[] { typeof(string) });
            method?.Invoke(form, new object[] { message });
        }
    }
}
