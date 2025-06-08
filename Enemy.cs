using System.Drawing;

public class Enemy : BaseElement
{
    public EnemyStats EnemyStats { get; set; }

    public Enemy(string symbol, Color forecolor, Color backcolor, EnemyStats stats)
        : base(symbol, forecolor, backcolor, true, true)
    {
        EnemyStats = stats;
    }

    public override void Print()
    {
        Console.Write(Output);
    }
    public override void Interact(Player player)
    {
    }
}
