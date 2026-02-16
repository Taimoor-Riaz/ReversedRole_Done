using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Hit Princess Hurts Player")]
public class Rule_HitPrincessHurtsPlayer : RuleCard
{
    [SerializeField] private int playerDamage = 2;

    public override void OnHit(HitEvent e, RuleContext ctx)
    {
        if (e.targetType != TargetType.Princess) return;
        ctx.healthDeltas.Add(-playerDamage);
    }
}
