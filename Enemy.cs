using System.Drawing;

public class Enemy : BaseElement
{
    public EnemyStats EnemyStats { get; set; }

    public Enemy(string symbol, Color forecolor, Color backcolor, EnemyStats stats)
        : base(symbol, forecolor, backcolor, true, true)
    {
        EnemyStats = stats;
    }

    public override void Interact(Player player)
    {
        Game? game = player.GetGame();
        object? form = GetFormIfExists(player);

        var battle = new Battle(player, this, game, player.X, player.Y);
        player.StartBattle(battle);
    }

    private object? GetFormIfExists(Player player)
    {
        return null;
    }
}
