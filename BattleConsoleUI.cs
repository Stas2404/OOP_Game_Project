using System;

public class BattleConsoleUI
{
    private readonly Battle battle;
    private readonly IGameUI ui;
    private bool inBattle = true;

    public BattleConsoleUI(Battle battle, IGameUI ui)
    {
        this.battle = battle;
        this.ui = ui;

        battle.OnMessage += ShowMessage;
        battle.OnBattleEnd += EndBattle;
        battle.OnBossDefeated += HandleBossDefeated;
    }

    public void Run()
    {
        while (inBattle)
        {
            ui.Clear();
            ui.WriteLine(battle.GetFormattedState());
            ui.WriteLine("\n1. Attack\n2. Heal" + (battle.CanMercy ? "\n3. Mercy" : ""));

            ConsoleKey key = ui.ReadKey();

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
                        ui.WriteLine("\n❌ Mercy is not allowed.");
                    }
                    break;
                default:
                    ui.WriteLine("\n❌ Invalid input. Use keys 1, 2" + (battle.CanMercy ? " or 3" : "") + ".");
                    break;
            }

            if (inBattle && playerActed)
            {
                battle.EnemyTurn();
            }

            if (inBattle)
            {
                ui.WriteLine("\nPress any key for next round...");
                ui.WaitForKey();
            }
        }
    }

    private void ShowMessage(string message)
    {
        ui.WriteLine(message);
    }

    private void EndBattle()
    {
        inBattle = false;
    }

    private void HandleBossDefeated()
    {
        ui.WriteLine("Press any key to finish the game, 'B' — to create custom level.");
        var key = ui.ReadKey();

        if (key == ConsoleKey.B)
        {
            var editor = new LevelEditor(ui);
            var editorUI = new LevelEditorConsoleUI(editor, ui);
            editorUI.Run();

            var menu = new GameConsoleUI(ui);

        }
        else
        {
            var menu = new GameConsoleUI(ui);

        }
    }
}
