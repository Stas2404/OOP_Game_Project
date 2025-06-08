public class BossAI
{
    public bool IsAttackBlocked { get; set; }
    public bool IsHealBlocked { get; set; }

    public int UnblockHealAttempts { get; set; }
    public int UnblockAttackAttempts { get; set; }

    private BaseAction attackAction;
    private BaseAction healAction;

    public BossAI()
    {
        attackAction = new AttackAction();
        healAction = new HealAction();
        attackAction.AnotherAction = healAction;
        healAction.AnotherAction = attackAction;
    }

    public void OnPlayerAttack()
    {
        attackAction.Apply(this);
    }

    public void OnPlayerHeal()
    {
        healAction.Apply(this);
    }
}
