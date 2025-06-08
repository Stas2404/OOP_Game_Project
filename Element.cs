using System;

internal class BattleConsoleUI
{
    private Battle battle;
    private bool inBattle = true;

    public BattleConsoleUI(Battle battle)
    {
        this.battle = battle;

        battle.OnMessage += ShowMessage;
        battle.OnBattleEnd += EndBattle;
        battle.OnBossDefeated += HandleBossDefeated; 
    }

    public void Run()
    {
        while (inBattle)
        {
            Console.Clear();
            Console.WriteLine(battle.GetFormattedState());

            Console.WriteLine("\n1. Attack\n2. Heal" + (battle.CanMercy ? "\n3. Mercy" : ""));
            ConsoleKey key = Console.ReadKey().Key;

            bool playerActed = false;

            switch (key)
            {
                case ConsoleKey.D1:
                    battle.PlayerAttack();
                    playerActed = true;
                    break;
                case ConsoleKey.D2:
                    battle.PlayerHeal();
                    playerActed = true;
                    break;
                case ConsoleKey.D3:
                    if (battle.CanMercy)
                    {
                        battle.PlayerMercy();
                        playerActed = true;
                    }
                    else
                    {
                        Console.WriteLine("\n❌ Mercy is not allowed.");
                    }
                    break;
                default:
                    Console.WriteLine("\n❌ Invalid input. Use keys 1, 2" + (battle.CanMercy ? " or 3" : "") + ".");
                    break;
            }

            if (inBattle && playerActed)
            {
                battle.EnemyTurn();
            }

            if (inBattle)
            {
                Console.WriteLine("\nPress any key for next round...");
                Console.ReadKey();
            }
        }
    }


    private void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    private void EndBattle()
    {
        inBattle = false;
    }
    private void HandleBossDefeated()
    {
        Console.WriteLine("Press any key to finish the game, 'B' — to create custom level.");
        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.B)
        {
            LevelEditor editor = new LevelEditor();
            editor.Start();
            Console.ReadKey();
            Environment.Exit(0);
        }
        else
        {
            Environment.Exit(0);
        }
    }
}
