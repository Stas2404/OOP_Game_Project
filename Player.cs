using System;
using System.Drawing;
#if WINDOWS
using WinFormsApp.Stubs;
#endif


public class Player : BaseElement
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public int Level { get; set; } = 1;
    public int WinsSinceLastLevel { get; set; } = 0;

    private Map map;
    private readonly IGameUI ui;
    private readonly Game game;
    private readonly object? form;

    public Game GetGame() => game;

    public Player(int x, int y, Map map, IGameUI ui, Game game, object? form = null)
        : base("P", Color.Green, Color.Black, true)
    {
        SetPosition(x, y);
        this.map = map;
        this.ui = ui;
        this.game = game;
        this.form = form;
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

        var nextElement = map[newY, newX];

        if (!nextElement.IsPassable)
            return;

        if (nextElement is Enemy enemy)
        {
            var battle = new Battle(this, enemy, game, newX, newY);

            battle.OnBattleEnd += () =>
            {
                if (battle.PlayerWon)
                {
                    map[newY, newX] = new EmptyTile();
                    map[Y, X] = new EmptyTile();
                    SetPosition(newX, newY);
                    map[Y, X] = this;
                    SoundManager.PlayMoveSound();
                }

                if (!map.HasEnemiesLeft())
                {
                    if (map.IsCustomLevel)
                    {
                        ShowMessage("🎉 You completed the custom level!");
                        DelayAndReturnToMainMenu();
                        return;
                    }

                    if (map.GameLevel.LevelNumber == 5)
                    {
                        ShowMessage("\n🏆 You defeated the boss!");
                    }
                    else
                    {
                        SetPosition(0, 0);
                        map.GenerateNextLevel(map.IsCustomLevel);
                    }
                }

                TryInvalidateForm();
            };

            StartBattle(battle);
            return;
        }

        map[Y, X] = new EmptyTile();
        SetPosition(newX, newY);
        map[Y, X] = this;

        nextElement.Interact(this);
        SoundManager.PlayMoveSound();

        if (!map.HasEnemiesLeft())
        {
            if (map.IsCustomLevel)
            {
                ShowMessage("🎉 You completed the custom level!");
                DelayAndReturnToMainMenu();
                return;
            }

            if (map.GameLevel.LevelNumber == 5)
            {
                ShowMessage("\n🏆 You defeated the boss!");
            }
            else
            {
                SetPosition(0, 0);
                map.GenerateNextLevel(map.IsCustomLevel);
            }
        }

        TryInvalidateForm();
    }

    public void StartBattle(Battle battle)
    {
        if (form == null)
        {
            var battleUI = new BattleConsoleUI(battle, ui);
            battleUI.Run();
        }
        else
        {
            TryInvokeFormMethod("ShowBattleUI", battle);
        }
    }

    public override void Interact(Player player) { }

    private void TryInvalidateForm()
    {
        if (form == null) return;

        var method = form.GetType().GetMethod("Invalidate", Type.EmptyTypes);
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

    private void DelayAndReturnToMainMenu()
    {
        if (form == null)
        {
            Thread.Sleep(1500);
            TryInvokeFormMethod("ReturnToMainMenu");
        }
        else
        {
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1500;
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                TryInvokeFormMethod("ReturnToMainMenu");
            };
            timer.Start();
        }
    }

    private void TryInvokeFormMethod(string methodName, object? parameter = null)
    {
        if (form == null) return;

        var method = parameter == null
            ? form.GetType().GetMethod(methodName)
            : form.GetType().GetMethod(methodName, new[] { parameter.GetType() });

        method?.Invoke(form, parameter == null ? null : new object[] { parameter });
    }
}
