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

    private readonly IGameUI ui;
    private readonly object? form;

    public Map CurrentMap => map;

    public Game(IGameUI ui, object? form = null)
    {
        this.ui = ui;
        this.form = form;

        map = new Map(ui, this, form);
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

        BaseElement.OnMessage -= ui.WriteLine;
        BaseElement.OnMessage += ui.WriteLine;
    }

    public void Init()
    {
        map.Init();
    }

    public void Run()
    {
        isRunning = true;

        if (form == null)
            ui.DrawMap(map);

        while (isRunning)
        {
            ConsoleKey key = ui.ReadKey();
            ui.Clear();

            if (moveMap.TryGetValue(key, out var move))
            {
                map.player.MovePlayer(move.dx, move.dy);
            }
            else
            {
                HandleSpecialKey(key);
            }

            if (form == null)
                ui.DrawMap(map);

            if (!map.HasEnemiesLeft())
            {
                if (isCustomLevel)
                {
                    ui.WriteLine("🎉 You completed the custom level!");
                    isRunning = false;
                    TryInvokeFormMethod("ReturnToMainMenu");
                    return;
                }

                map.GenerateNextLevel();
                if (form == null)
                    ui.DrawMap(map);
            }
        }
    }

    private void HandleSpecialKey(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.F5:
                GameSaver.Save(map, savePath);
                ui.WriteLine("Game saved.");
                Thread.Sleep(1000);
                break;

            case ConsoleKey.F6:
                GameSaver.Load(map, savePath);
                ui.WriteLine("Game loaded.");
                Thread.Sleep(1000);
                break;

            case ConsoleKey.F3:
                if (map.GameLevel.LevelNumber == 6)
                {
                    GameSaver.Save(map, customPath);
                    ui.WriteLine("Custom level saved.");
                }
                Thread.Sleep(1000);
                break;

            case ConsoleKey.F4:
                if (bossDefeated)
                {
                    if (File.Exists(customPath))
                    {
                        map = new Map(ui, this, form);
                        GameSaver.Load(map, customPath);
                        map.IsCustomLevel = true;
                        isCustomLevel = true;
                        ui.Clear();
                        ui.WriteLine("Custom level loaded.");
                        Thread.Sleep(1000);
                        Run();
                    }
                    else
                    {
                        ui.WriteLine("Custom level file not found.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    ui.WriteLine("Defeat the boss to access the custom level!");
                    Thread.Sleep(1000);
                }
                break;
        }
    }

    public void SetBossDefeated()
    {
        bossDefeated = true;
    }

    public bool RunCustomLevel()
    {
        if (!File.Exists(customPath))
        {
            ui.WriteLine("Custom level not found.");
            return false;
        }

        map = new Map(ui, this, form);
        map.IsCustomLevel = true;
        GameSaver.Load(map, customPath);
        isCustomLevel = true;
        return true;
    }

    private void TryInvokeFormMethod(string methodName, object? parameter = null)
    {
        if (form == null) return;

        var method = parameter == null
            ? form.GetType().GetMethod(methodName)
            : form.GetType().GetMethod(methodName, new[] { parameter.GetType() });

        if (method != null)
        {
            method.Invoke(form, parameter == null ? null : new object[] { parameter });
        }
    }
}
