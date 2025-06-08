public class EnemyStats
{
    public int Level { get; private set; }
    public bool IsTank { get; private set; }

    public int BaseHP => IsTank ? 150 : 100;
    public int MaxHP => BaseHP + (Level - 1) * 15;

    public int DamageMin => 20 + (Level - 1) * 5;
    public int DamageMax => DamageMin + 5;

    public EnemyStats(int level, bool isTank = false)
    {
        Level = level;
        IsTank = isTank;
    }
}
