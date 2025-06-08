abstract class BaseAction
{
    public int Combo { get; set; } = 0;
    public BaseAction AnotherAction { get; set; }

    public abstract void Apply(BossAI bossAI);
}
class AttackAction : BaseAction
{
    public override void Apply(BossAI bossAI)
    {
        Combo++;
        AnotherAction.Combo = 0;

        if (bossAI.IsHealBlocked)
        {
            bossAI.UnblockHealAttempts++;
            if (bossAI.UnblockHealAttempts >= 2)
            {
                bossAI.IsHealBlocked = false;
                bossAI.UnblockHealAttempts = 0;
            }
        }

        if (Combo >= 3 && !bossAI.IsAttackBlocked)
        {
            bossAI.IsAttackBlocked = true;
            bossAI.UnblockAttackAttempts = 0;
            Combo = 0;
        }
    }
}

class HealAction : BaseAction
{
    public override void Apply(BossAI bossAI)
    {
        Combo++;
        AnotherAction.Combo = 0;

        if (bossAI.IsAttackBlocked)
        {
            bossAI.UnblockAttackAttempts++;
            if (bossAI.UnblockAttackAttempts >= 2)
            {
                bossAI.IsAttackBlocked = false;
                bossAI.UnblockAttackAttempts = 0;
            }
        }

        if (Combo >= 3 && !bossAI.IsHealBlocked)
        {
            bossAI.IsHealBlocked = true;
            bossAI.UnblockHealAttempts = 0;
            Combo = 0;
        }
    }
}
