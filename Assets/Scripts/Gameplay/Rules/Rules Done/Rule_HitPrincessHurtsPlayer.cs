using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Hit Princess Hurts Player")]
public class Rule_HitPrincessHurtsPlayer : RuleCard
{
    [SerializeField] private int playerDamage = 2;
    [SerializeField] private int princessKillDamage = 999; // enough to kill
    public override void OnHit(HitEvent e, RuleContext ctx)
    {
        if (e.targetType != TargetType.Princess) return;
        ctx.healthDeltas.Add(-playerDamage);
        // ✅ kill THIS princess instance
        if (e.target != null)
        {
            ctx.damages.Add(new DamageRequest
            {
                target = e.target,
                damage = princessKillDamage
            });
        }
    }
}
