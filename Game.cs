using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public class Game
{
    private Map map;
    private bool isRunning;
    private string savePath;
    private string customPath;

    private Dictionary<ConsoleKey, (int dx, int dy)> moveMap;

    private bool bossDefeated = false;
    private bool isCustomLevel = false;

    public Game()
    {
        map = new Map();
        isRunning = true;
        savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "save.txt");
        customPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customlvl.txt");



        moveMap = new Dictionary<ConsoleKey, (int dx, int dy)>
        {
            [ConsoleKey.W] = (0, -1),
            [ConsoleKey.S] = (0, 1),
            [ConsoleKey.A] = (-1, 0),
            [ConsoleKey.D] = (1, 0)
        };
    }

    public void Init()
    {
        map.Init();
        map.Print();
    }

    public void Run()
    {
        isRunning = true;
        while (isRunning)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.Clear();

            if (moveMap.TryGetValue(key.Key, out var move))
            {
                map.player.MovePlayer(move.dx, move.dy);
            }
            else
            {
                HandleSpecialKey(key.Key);
            }

            map.Print();

            if (!map.HasEnemiesLeft())
            {
                if (isCustomLevel)
                {
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    map.GenerateNextLevel();
                }
            }
        }
    }


    private void HandleSpecialKey(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.F5:
                GameSaver.Save(map, savePath);
                Console.WriteLine("Game saved.");
                Thread.Sleep(1000);
                break;

            case ConsoleKey.F6:
                GameSaver.Load(map, savePath);
                Console.WriteLine("Game loaded.");
                Thread.Sleep(1000);
                break;

            case ConsoleKey.F3:
                if (map.GameLevel.LevelNumber == 6)
                {
                    GameSaver.Save(map, customPath);
                    Console.WriteLine("Custom level saved.");
                }
                Thread.Sleep(1000);
                break;


            case ConsoleKey.F4:
                if (bossDefeated)
                {
                    if (File.Exists(customPath))
                    {
                        map = new Map();
                        GameSaver.Load(map, customPath);
                        Console.Clear();
                        Console.WriteLine("Custom level loaded.");
                        Thread.Sleep(1000);
                        Run();
                    }
                    else
                    {
                        Console.WriteLine("Custom level file not found.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("Defeat the boss to access the custom level!");
                    Thread.Sleep(1000);
                }
                break;

            case ConsoleKey.Escape:
                isRunning = false;
                Game game = new Game();
                game.Menu();
                break;

        }
    }
    public void SetBossDefeated()
    {
        bossDefeated = true;
    }
    public void RunCustomLevel()
    {
        if (!File.Exists(customPath))
        {
            Console.WriteLine("Custom level not found.");
            Thread.Sleep(1000);
            return;
        }

        map = new Map();
        map.IsCustomLevel = true;
        GameSaver.Load(map, customPath);
        isCustomLevel = true;
        Console.Clear();
        Run();
    }

    public void Menu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Goblins Dungeon");
            Console.WriteLine("1. Play");
            Console.WriteLine("2. Custom Level");
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect an option (1-3): ");

            ConsoleKeyInfo key = Console.ReadKey(true);
            Console.WriteLine(key.KeyChar);

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Init();
                    Run();
                    return;
                case ConsoleKey.D2:
                    LevelEditor editor = new LevelEditor();
                    editor.Start();
                    return;
                case ConsoleKey.D3:
                    Environment.Exit(0);
                    return;
                default:
                    Console.WriteLine("\nInvalid input. Please press 1, 2, or 3.");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }

}
