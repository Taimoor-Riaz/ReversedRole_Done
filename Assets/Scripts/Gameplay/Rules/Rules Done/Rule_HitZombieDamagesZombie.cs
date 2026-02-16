using UnityEngine;

[CreateAssetMenu(menuName = "Rules/Hit Zombie Damages Zombie")]
public class Rule_HitZombieDamagesZombie : RuleCard
{
    public override void OnHit(HitEvent e, RuleContext ctx)
    {
        if (e.targetType != TargetType.Zombie) return;

        ctx.damages.Add(new DamageRequest
        {
            targetType = TargetType.Zombie,
            damage = e.baseDamage
        });
    }
}
