using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Hit Princess Gives Score")]
public class Rule_HitPrincessGivesScore : RuleCard
{
    [SerializeField] private int healAmount = 2;
    [SerializeField] private int princessKillDamage = 999; // enough to kill
    public override void OnHit(HitEvent e, RuleContext ctx)
    {
        if (e.targetType != TargetType.Princess) return;
        ctx.healthDeltas.Add(+healAmount);

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
