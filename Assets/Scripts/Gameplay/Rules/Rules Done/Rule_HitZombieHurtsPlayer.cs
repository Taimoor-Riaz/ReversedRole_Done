using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Hit Zombie Hurts Player")]
public class Rule_HitZombieHurtsPlayer : RuleCard
{
    [SerializeField] private int playerDamage = 1;

    public override void OnHit(HitEvent e, RuleContext ctx)
    {
        if (e.targetType != TargetType.Zombie) return;
        ctx.healthDeltas.Add(-playerDamage);
    }
}
