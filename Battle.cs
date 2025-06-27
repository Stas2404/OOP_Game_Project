public class Battle
{
    private Player player;
    private Enemy enemy;
    private int playerHP;
    private int playerMaxHP;
    private int enemyHP;
    private int enemyMaxHP;
    private int enemyLevel;
    private string enemyName;
    private bool isBoss;
    private Game game;
    private int enemyX;
    private int enemyY;

    private BossAI bossAI;

    public bool PlayerWon { get; private set; } = true;
    public Enemy Enemy { get; private set; }

    public bool CanMercy => !isBoss;
    public string EnemyName => enemyName;
    public int EnemyLevel => enemyLevel;



    public event Action<string> OnMessage;
    public event Action OnBattleEnd;
    public event Action OnBossDefeated;

    public Battle(Player player, Enemy enemy, Game game, int enemyX, int enemyY)
    {
        this.player = player;
        this.enemy = enemy;
        this.game = game;
        this.enemyX = enemyX;
        this.enemyY = enemyY;

        SoundManager.PlayBattleMusic();

        playerHP = 100 + (player.Level - 1) * 20;
        playerMaxHP = playerHP;

        isBoss = enemy.Output == "B";
        if (isBoss)
        {
            bossAI = new BossAI();
            enemyLevel = 25;
            enemyMaxHP = 400;
            enemyHP = 400;
            enemyName = "Boss";
        }
        else
        {
            if (enemy.EnemyStats != null)
            {
                enemyLevel = enemy.EnemyStats.Level;
                enemyMaxHP = enemy.EnemyStats.MaxHP;
            }
            else
            {
                enemyLevel = 1;
                enemyMaxHP = 100;
            }

            enemyHP = enemyMaxHP;
            enemyName = (enemy.Output == "T") ? "Tank" : "Enemy";
        }
    }

    public string GetFormattedState()
    {
        string output = "";
        output += $"P{"".PadRight(50)}{enemy.Output}\n";
        output += $"Player's Level: {player.Level}".PadRight(50) + $"{enemyName}'s Level: {enemyLevel}\n";
        output += $"Player's HP: {playerHP}/{playerMaxHP}".PadRight(50) +
                  $"{enemyName}'s HP: {enemyHP}/{enemyMaxHP}\n";

        if (isBoss)
        {
            if (bossAI.IsAttackBlocked)
                output += "\n⚠️ Boss has blocked your attack for 2 turns!\n";
            if (bossAI.IsHealBlocked)
                output += "⚠️ Boss has blocked your healing for 2 turns!\n";
        }

        return output;
    }

    public void PlayerAttack()
    {
        if (isBoss && bossAI.IsAttackBlocked)
        {
            OnMessage?.Invoke("❌ Your attack is blocked!");
            return;
        }

        int damage = 50 + (player.Level - 1) * 10;
        enemyHP -= damage;
        OnMessage?.Invoke($"✅ You dealt {damage} damage to the {enemyName.ToLower()}!");

        if (isBoss) bossAI.OnPlayerAttack();

        CheckVictory();
    }

    public void PlayerHeal()
    {
        if (isBoss && bossAI.IsHealBlocked)
        {
            OnMessage?.Invoke("❌ Your healing is blocked!");
            return;
        }

        int healAmount = 10 + (player.Level - 1) * 10;
        playerHP = Math.Min(playerHP + healAmount, playerMaxHP);
        OnMessage?.Invoke($"\nYou healed for {healAmount} HP!");

        if (isBoss) bossAI.OnPlayerHeal();
    }

    public void PlayerMercy()
    {
        if (isBoss)
        {
            OnMessage?.Invoke("You cannot show mercy to the boss!");
            return;
        }

        OnMessage?.Invoke("\nYou showed mercy. The enemy flees.");
        enemy.Output = ".";
        EndBattle(true);
    }

    public void EnemyTurn()
    {
        if (enemyHP <= 0 || playerHP <= 0) return;

        int damage = isBoss
            ? new Random().Next(40, 66)
            : new Random().Next(enemy.EnemyStats?.DamageMin ?? 20, (enemy.EnemyStats?.DamageMax ?? 30) + 1);

        playerHP -= damage;
        OnMessage?.Invoke($"💥 {enemyName} attacks and deals {damage} damage!");

        if (playerHP <= 0)
        {
            PlayerWon = false;
            OnMessage?.Invoke("☠️ You have been defeated!");
            EndBattle(false);
        }
    }

    private void CheckVictory()
    {
        if (enemyHP <= 0)
        {
            if (isBoss)
            {
                OnMessage?.Invoke("🎉 Congratulations! You defeated the boss!");
                OnBossDefeated?.Invoke();
                game.SetBossDefeated();
                EndBattle(true);
                return;
            }

            OnMessage?.Invoke($"You defeated the {enemyName.ToLower()}!");

            game.CurrentMap[player.Y, player.X] = new EmptyTile();

            player.SetPosition(enemyX, enemyY);
            game.CurrentMap[enemyY, enemyX] = player;

            player.WinsSinceLastLevel++;
            if (player.WinsSinceLastLevel >= player.Level)
            {
                player.Level++;
                player.WinsSinceLastLevel = 0;
                OnMessage?.Invoke($"You leveled up! Now Level {player.Level}!");
            }

            EndBattle(true);
        }
    }

    private void EndBattle(bool playerWon)
    {
        PlayerWon = playerWon;

        if (!playerWon)
        {
            OnMessage?.Invoke("You lost the battle.");
        }

        OnBattleEnd?.Invoke();
    }
}
