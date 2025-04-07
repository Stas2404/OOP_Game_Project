using System;
using System.Collections.Generic;

internal class Battle
{
    private Player player;
    private Element enemy;
    private bool inBattle;
    public bool PlayerWon { get; private set; } = true;
    private List<int> playerLevelsHistory;

    public Battle(Player player, Element enemy, List<int> playerLevelsHistory)
    {
        this.player = player;
        this.enemy = enemy;
        this.playerLevelsHistory = playerLevelsHistory;
        inBattle = true;
        StartBattle();
    }

    private void StartBattle()
    {
        int playerHP = 100;
        int enemyHP = 100;
        int enemyLevel = 1;

        while (inBattle)
        {
            Console.Clear();
            Console.WriteLine($"P                                       E");
            Console.WriteLine($"Player's Level: {player.Level}");
            Console.WriteLine($"Enemy's Level: {enemyLevel}");
            Console.WriteLine($"Player's HP: {playerHP}/100");
            Console.WriteLine($"Enemy's HP: {enemyHP}/100");
            Console.WriteLine("\n1. Attack\n2. Heal\n3. Mercy");

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("\nYou attack the enemy!");
                    enemyHP -= 75;
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("\nYou heal yourself!");
                    playerHP = Math.Min(playerHP + 10, 100);
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("\nYou chose mercy. The battle ends.");
                    inBattle = false;
                    enemy.Output = "#";
                    return;
            }

            if (enemyHP <= 0)
            {
                Console.WriteLine("\nYou defeated the enemy!");
                inBattle = false;
                enemy.Output = "#";
                return;
            }

            Console.WriteLine("\nEnemy's turn...");
            playerHP -= 20;

            if (playerHP <= 0)
            {
                Console.WriteLine("\nYou have been defeated!");
                inBattle = false;
                PlayerWon = false;

                if (playerLevelsHistory.Count > 0)
                {
                    player.Level = playerLevelsHistory[^1];
                    playerLevelsHistory.RemoveAt(playerLevelsHistory.Count - 1);
                }

                return;
            }

            Console.WriteLine("\nPress any key for the next round...");
            Console.ReadKey();
        }
    }
}
